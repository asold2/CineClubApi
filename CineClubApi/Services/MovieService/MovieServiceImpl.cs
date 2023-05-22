using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.MovieResult;
using CineClubApi.Models;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.MovieRepository;

namespace CineClubApi.Services.MovieService;

public class MovieServiceImpl :  IMovieService
{

    private readonly IMovieRepository _movieRepository;
    private readonly IListRepository _listRepository;

    public MovieServiceImpl(IMovieRepository movieRepository, IListRepository listRepository)
    {
        _movieRepository = movieRepository;
        _listRepository = listRepository;
    }
    
    
    public async Task<Guid> SaveOrGetMovieDao(int tmdbMovieId)
    {

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