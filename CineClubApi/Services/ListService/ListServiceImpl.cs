using System.Diagnostics.Contracts;
using AutoMapper;
using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.ListResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListTagService;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Services.ListService;

public class ListServiceImpl : ListService, IListService
{
    private readonly ILikedListService _likedListService;
    public ListServiceImpl(IListRepository listRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ITagRepository tagRepository,
        IPaginator paginator,
        ITMDBMovieService movieService,
        ITMDBPeopleService peopleService,
        ITMDBGenreService genreService, 
        ILikedListService likedListService) : base(listRepository, userRepository, mapper, tagRepository, paginator, movieService, peopleService, genreService)
    {
        _likedListService = likedListService;
    }


    public async Task<IList<UpdateListDto>> GetListsByUserId(Guid userId)
    {
        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser == null)
        {
            return null;
        }


        var lists = await _listRepository.GetAllListsByUserId(neededUser.Id);

        lists = lists.Where(x => x.Name != "Liked Movies" && x.Name != "Watched Movies").ToList();

        var result = _mapper.ProjectTo<UpdateListDto>(lists.AsQueryable()).ToList();

        foreach (var tempList in result)
        {
            var l = await AssignImageToList(tempList);
            tempList.BackdropPath = l.BackdropPath;
        }

        return result;
    }

    
    public async Task<List<ListDto>> GetListsByTags(List<Guid> tagIds)
    {
        var listOfNeededTags = new List<Tag>();

        foreach (var id in tagIds)
        {
            var tag = await _tagRepository.GetTagWithListsById(id);
            if (tag == null)
            {
                continue;
            }

            listOfNeededTags.Add(tag);
        }

        var listOfNeededLists = new List<List>();

        foreach (var tag in listOfNeededTags)
        {
            listOfNeededLists.AddRange(tag.Lists);
        }

        var result = _mapper.ProjectTo<ListDto>(listOfNeededLists.AsQueryable()).ToList();

        //getting only public lists
        result = result.Where(x => x.Public).ToList();

        return result;
    }


    public async Task<PaginatedResult<UpdateListDto>> GetAllLists(int page, int start, int end)
    {
        var lists = await _listRepository.GetAllPublicLists();

        var result = _mapper.ProjectTo<UpdateListDto>(lists.AsQueryable()).ToList();

        var paginatedResult = await _paginator.PaginateUpdatedListDto(result, page, start, end);

        foreach (var tempList in paginatedResult.Result)
        {
            var l = await AssignImageToList(tempList);
            tempList.BackdropPath = l.BackdropPath;
        }

        return paginatedResult;
    }


    public async Task<DetailedListDto> GetListsById(Guid listId)
        {
            var neededList = await _listRepository.GetListByIdWithEverythingIncluded(listId);

            if (neededList == null)
            {
                return new DetailedListDto();
            }

            var tagDtos = _mapper.ProjectTo<TagForListDto>(neededList.Tags.AsQueryable()).ToList();

            var result = _mapper.Map<DetailedListDto>(neededList);

            result.TagsDtos = tagDtos;

            foreach (var movie in neededList.MovieDaos)
            {
                var tmdbMovie = await _movieService.getMovieById(movie.tmdbId);
                var genreDtos = new List<int>();

                foreach (var genre in tmdbMovie.Genres)
                {
                    genreDtos.Add(genre.Id);
                }

                var movieDto = _mapper.Map<MovieForListDto>(tmdbMovie);
                movieDto.GenreIds = genreDtos;
                result.MovieDtos.Add(movieDto);
            }
            
            return result;
        }


    public async Task<List<MoviePersonDto>> GetTop5ActorsByListId(Guid listId)
        {
            var neededList = await _listRepository.GetListByIdWithEverythingIncluded(listId);


            var topActorsFromEachMove = new List<MoviePersonDto>();

            foreach (var movie in neededList.MovieDaos)
            {
                var tmdbMovie = await _movieService.getMovieById(movie.tmdbId);

                var top15Actors = await _peopleService.GetAllActors(movie.tmdbId);

                topActorsFromEachMove.AddRange(top15Actors);
            }

            var top5Actors = topActorsFromEachMove
                .OrderByDescending(actor => actor.Popularity)
                .Take(5)
                .ToList();


            return top5Actors;
        }

    public async Task<List<MoviePersonDto>> GetTop5DirectorsByListId(Guid listId)
        {
            var neededList = await _listRepository.GetListByIdWithEverythingIncluded(listId);

            var topDirectorsFromEachMove = new List<MoviePersonDto>();

            foreach (var movie in neededList.MovieDaos)
            {
                var tmdbMovie = await _movieService.getMovieById(movie.tmdbId);

                var movieDirectors = await _peopleService.GetMovieDirectors(movie.tmdbId);

                topDirectorsFromEachMove.AddRange(movieDirectors);
            }

            var top5Actors = topDirectorsFromEachMove
                .OrderByDescending(director => director.Popularity)
                .Take(5)
                .ToList();


            return top5Actors;
        }


}