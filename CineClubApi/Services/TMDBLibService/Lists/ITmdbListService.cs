using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;

namespace CineClubApi.Services.TMDBLibService.Lists;

public interface ITmdbListService
{
    Task<List<MovieForListDto>> GetPopularMovies(int page, int? start, int? end);
    Task<List<MovieForListDto>> GetTopRatedMovies(int page, int? start, int? end);
    Task<List<MovieForListDto>> GetUpcomingMovies(int page, int? start, int? end);
    Task<List<MovieForListDto>> GetTrendingMovies(TimePeriod period, int page, int? start, int? end);
}