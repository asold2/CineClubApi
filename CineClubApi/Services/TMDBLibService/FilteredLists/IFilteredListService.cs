using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using TMDbLib.Objects.Discover;

namespace CineClubApi.Services.TMDBLibService.FilteredLists;

public interface IFilteredListService
{
    Task<List<MovieForListDto>> GetFilteredListOfMovies(List<int>? genreIds,
        int? year,
        DateTime? releasedAfter,
        DateTime? releasedBefore,
        double? leastVoteAverage,
        string? language,
        DiscoverMovieSortBy? sortBy,
        bool? includeAdultMovies);
}