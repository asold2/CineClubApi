using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Helpers;
using CineClubApi.Models;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;

namespace CineClubApi.Services.ListService.WatchedList;

public class WatchedListServiceImpl : ListService, IWatchedListService
{
    public WatchedListServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository, IPaginator paginator, ITMDBMovieService movieService, ITMDBPeopleService peopleService, ITMDBGenreService genreService) : base(listRepository, userRepository, mapper, tagRepository, paginator, movieService, peopleService, genreService)
    {
    }
    
    public async Task<UpdateListDto> GetUsersWatchedList(Guid userId)
    {
        var allLists = await _listRepository.GetAllListsByUserId(userId);

        var neededUser = await _userRepository.GetUserById(userId);

        if (neededUser == null)
        {
            return null;
        }

        var watchedList = allLists.FirstOrDefault(x => x.Name == "Watched Movies");

        if (watchedList == null)
        {
            var newWatchedList = new List
            {
                Name = "Watched Movies",
                Public = false,
                CreatorId = userId,
                Creator = neededUser
            };
            await _listRepository.CreateList(newWatchedList);

            return _mapper.Map<UpdateListDto>(newWatchedList);
        }

        var result = _mapper.Map<UpdateListDto>(watchedList);

        result = await AssignImageToList(result);


        return result;
    }

    
    
    public async Task<bool> MovieBelongsToWatched(Guid userId, int tmdbId)
    {
        var watchedList = await GetUsersWatchedList(userId);

        var moviesInWatchedList = await _listRepository.GetListWithMovies(watchedList.Id);

        return moviesInWatchedList.MovieDaos.Any(x => x.tmdbId == tmdbId);
    }
    
    
    
}