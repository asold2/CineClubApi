using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Models;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.MovieRepository;
using CineClubApi.Services.ListService;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListService.WatchedList;
using CineClubApi.Services.TMDBLibService;

namespace CineClubApi.Services.MovieService.MovieListService;

public class MovieListServiceImpl : MovieService, IMovieListService
{
    public MovieListServiceImpl(IMovieRepository movieRepository, IListRepository listRepository, ITMDBMovieService tmdbMovieService, IMapper mapper, IListService listService, ILikedListService likedListService, IWatchedListService watchedListService) : base(movieRepository, listRepository, tmdbMovieService, mapper, listService, likedListService, watchedListService)
    {
    }
    
    
    
    public async Task<List<SimpleListDto>> GetUsersListsToWhichMovieBelongs(Guid userId, int tmdbId)
    {
        var neededMovie = await  _movieRepository.GetMovieByTmdbId(tmdbId);

        if (neededMovie==null)
        {
            return null;
        }
        
        var listsMovieBelongsTo =  neededMovie.Lists
            .Where(x=>x.CreatorId==userId)
            .ToList();

        var result = _mapper.ProjectTo<UpdateListDto>(listsMovieBelongsTo.AsQueryable()).ToList();



        var listsToReturn = _mapper.ProjectTo<SimpleListDto>(result.AsQueryable()).ToList();
        
        return listsToReturn;
    }

    public async Task<List<SimpleListDto>> GetUsersListsMovieDoesNotBelongTo(Guid userId, int tmdbId)
    {
        var neededMovie = await  _movieRepository.GetMovieByTmdbId(tmdbId);

        if (neededMovie==null)
        {
            return null;
        }
        
        var listsMovieBelongsTo =  neededMovie.Lists
            .Where(x=>x.CreatorId==userId)
            .ToList();


        var allUsersLists = await _listRepository.GetAllListsByUserId(userId);

        foreach (var list in listsMovieBelongsTo)
        {
            allUsersLists.Remove(list);
        }
        
        var result = _mapper.ProjectTo<UpdateListDto>(allUsersLists.AsQueryable()).ToList();
        
        var listsToReturn = _mapper.ProjectTo<SimpleListDto>(result.AsQueryable()).ToList();
        
        return listsToReturn;
    }

    public async Task<List<UpdateListDto>> GetAllListsMoviebelongsTo(int tmdbId)
    {
        var neededMovie = await  _movieRepository.GetMovieByTmdbId(tmdbId);
        
        var listsMovieBelongsTo =  neededMovie.Lists
            .Where(x=>x.Public)
            .ToList();
        
        var result = _mapper.ProjectTo<UpdateListDto>(listsMovieBelongsTo.AsQueryable()).ToList();
        
        foreach (var tempList in result)
        {
            var t = await _listService.AssignImageToList(tempList);

            tempList.BackdropPath = t.BackdropPath;

        }

        return result;
    }


}