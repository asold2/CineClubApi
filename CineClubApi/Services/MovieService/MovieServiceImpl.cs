using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.MovieResult;
using CineClubApi.Models;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.MovieRepository;
using CineClubApi.Services.ListService;
using CineClubApi.Services.TMDBLibService;

namespace CineClubApi.Services.MovieService;

public class MovieServiceImpl :  IMovieService
{

    private readonly IMovieRepository _movieRepository;
    private readonly IListRepository _listRepository;
    private readonly ITMDBMovieService _tmdbMovieService;
    private readonly IMapper _mapper;
    private readonly IListService _listService;

    public MovieServiceImpl(IMovieRepository movieRepository,
        IListRepository listRepository,
        ITMDBMovieService tmdbMovieService,
        IMapper mapper,
        IListService listService)
    {
        _movieRepository = movieRepository;
        _listRepository = listRepository;
        _tmdbMovieService = tmdbMovieService;
        _mapper = mapper;
        _listService = listService;
    }
    
    
    public async Task<Guid> SaveOrGetMovieDao(int tmdbMovieId)
    {
        var result = await _tmdbMovieService.getMovieById(tmdbMovieId);
        
        if ( result == null)
        {
            return Guid.Empty;
        }
        

        MovieDao movieDaoToSave = new MovieDao
        {
            Id = Guid.NewGuid(),
            tmdbId = tmdbMovieId
        };

        var movieSavedResult = await _movieRepository.SaveMovieDao(movieDaoToSave);


        return movieSavedResult;

    }

    public async Task<ServiceResult> AddMovieToList(Guid listId, Guid userId, int tmdbId)
    {

        if (await _listRepository.GetListById(listId) == null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "List not found!"
            };
        }
        
        if (!await UserHasRightTOUpdateList(listId,userId))
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User has no right to update this list!"
            };
        }
        
        var neededMovieId = await SaveOrGetMovieDao(tmdbId);

        if (neededMovieId == Guid.Empty)
        {
            return new ServiceResult
            {
                Result = "Movie with this TMDB id not found",
                StatusCode = 400
            };
        }

        await _movieRepository.AddMovieToList(listId, neededMovieId);

        return new MovieAddedToListResult();

    }

    public async Task<ServiceResult> DeleteMovieFromList(Guid listId, Guid userId, int tmdbId)
    {
        if (!await UserHasRightTOUpdateList(listId,userId))
        {
            return new ServiceResult
            {
                StatusCode = 403,
                Result = "User has no right to update this list!"
            };
        }

        var neededList = await _listRepository.GetListWithMovies(listId);

        if (neededList==null)
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "List not found!"
            }; 
        }

        if (!neededList.MovieDaos.Any(x => x.tmdbId == tmdbId))
        {
            return new ServiceResult
            {
                StatusCode = 400,
                Result = "Movie does not belong to this list!"
            }; 
        }

        var neededMovie = await _movieRepository.GetMovieByTmdbId(tmdbId);


        neededList.MovieDaos.Remove(neededMovie);

        await _listRepository.UpdateList(neededList);
        
        return new ServiceResult
        {
            StatusCode = 200,
            Result = "Movie successfully deleted from list!"
        }; 
    }

    public async Task<List<UpdateListDto>> GetUsersListsToWhichMovieBelongs(Guid userId, int tmdbId)
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

        foreach (var tempList in result)
        {
            var t = await _listService.AssignImageToList(tempList);

            tempList.BackdropPath = t.BackdropPath;

        }

        return result;
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

    public async Task<ServiceResult> AddMovieToLikedList(Guid userid, int tmdbId)
    {
        var likedList =await  _listService.GetUsersLikedList(userid);

        if (likedList==null)
        {
            return new ServiceResult
            {
                Result = "List of liked movies not found!",
                StatusCode = 400
            };
        }
        
        var result = await AddMovieToList(likedList.Id, userid, tmdbId);

        return result;

    }
    
    public async Task<ServiceResult> AddMovieToWatchedList(Guid userid, int tmdbId)
    {
        var watchedList = await  _listService.GetUsersWatchedList(userid);

        if (watchedList==null)
        {
            return new ServiceResult
            {
                Result = "List of watched movies not found!",
                StatusCode = 400
            };
        }
        
        var result = await AddMovieToList(watchedList.Id, userid, tmdbId);

        return result;

    }


    private async Task<bool> UserHasRightTOUpdateList(Guid listId, Guid userId)
    {
        var neededList = await _listRepository.GetListById(listId);

        if (neededList.CreatorId == userId)
        {
            return true;
        }

        return false;
    }
}