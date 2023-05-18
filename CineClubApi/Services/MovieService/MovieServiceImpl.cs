using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.AccountResults;
using CineClubApi.Common.ServiceResults.MovieResult;
using CineClubApi.Models;
using CineClubApi.Repositories.MovieRepository;

namespace CineClubApi.Services.MovieService;

public class MovieServiceImpl : IMovieService
{

    private readonly IMovieRepository _movieRepository;

    public MovieServiceImpl(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
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

    public async Task<ServiceResult> AddMovieToList(Guid listId, int tmdbId)
    {
        var neededMovieId = await SaveOrGetMovieDao(tmdbId);

        await _movieRepository.AddMovieToList(listId, neededMovieId);

        return new MovieAddedToListResult();

    }
}