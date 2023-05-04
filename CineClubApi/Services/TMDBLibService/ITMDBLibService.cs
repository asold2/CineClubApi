using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService;

public interface ITMDBLibService
{
    Task getAllMovies();

    Task<MovieDao> GetMovieByKeyword(string keyword);

}