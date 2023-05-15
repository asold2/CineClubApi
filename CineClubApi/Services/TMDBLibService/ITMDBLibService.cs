using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService;

public interface ITMDBLibService
{
    Task<List<MovieForListDto>> GetMoviesByKeyword(string keyword);
    Task<DetailedMovieDto> getMovieById(int id);
    Task<byte[]> GetMovieImage(string url);
    Task<List<MovieForListDto>> GetPopularMovies(int page, int start, int end);
    Task<List<MovieForListDto>> GetTopRatedMovies(int page, int start, int end);
    Task<List<MovieForListDto>> GetUpcomingMovies(int page, int start, int end);
    Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period, int page, int start, int end);

}