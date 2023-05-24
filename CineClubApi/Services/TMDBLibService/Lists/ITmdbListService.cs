using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;

namespace CineClubApi.Services.TMDBLibService.Lists;

public interface ITmdbListService
{
    Task<PaginatedListOfMovies> GetPopularMovies(int page, int? start, int? end);
    Task<PaginatedListOfMovies> GetTopRatedMovies(int page, int? start, int? end);
    Task<PaginatedListOfMovies> GetUpcomingMovies(int page, int? start, int? end);
    Task<PaginatedListOfMovies> GetTrendingMovies(TimePeriod period, int page, int? start, int? end);
}