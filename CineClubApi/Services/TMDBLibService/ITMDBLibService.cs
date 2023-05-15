using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService;

public interface ITMDBLibService
{
    Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword);
    Task<DetailedMovieDto> getMovieById(int id);
    Task<byte[]> GetMovieImage(string url);
    Task<List<MovieForListDto>> GetPopularMovies();
    Task<List<MovieForListDto>> GetTopRatedMovies();
    Task<List<MovieForListDto>> GetUpcomingMovies();
    Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period);

}