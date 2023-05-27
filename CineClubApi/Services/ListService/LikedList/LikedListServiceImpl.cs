using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Helpers;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.LikeService;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;

namespace CineClubApi.Services.ListService.LikedList;

public class LikedListServiceImpl: ListService, ILikedListService
{
    public LikedListServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository, IPaginator paginator, ITMDBMovieService movieService, ITMDBPeopleService peopleService, ITMDBGenreService genreService) : base(listRepository, userRepository, mapper, tagRepository, paginator, movieService, peopleService, genreService)
    {
    }
    
    public async Task<UpdateListDto> GetUsersLikedList(Guid userId)
    {
        var allLists = await _listRepository.GetAllListsByUserId(userId);

        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser == null)
        {
            return null;
        }

        var likedList = allLists.FirstOrDefault(x => x.Name == "Liked Movies");

        if (likedList == null)
        {
            var newLikedList = new List
            {
                Name = "Liked Movies",
                Public = false,
                CreatorId = userId,
                Creator = neededUser
            };
            await _listRepository.CreateList(newLikedList);

            return _mapper.Map<UpdateListDto>(newLikedList);
        }

        var result = _mapper.Map<UpdateListDto>(likedList);

        result = await AssignImageToList(result);

        return result;
    }
    
    public async Task<bool> MovieBelongsToLiked(Guid userId, int tmdbId)
    {
        var watchedList = await GetUsersLikedList(userId);

        var moviesInWatchedList = await _listRepository.GetListWithMovies(watchedList.Id);

        return moviesInWatchedList.MovieDaos.Any(x => x.tmdbId == tmdbId);
    }

    
    
    
}