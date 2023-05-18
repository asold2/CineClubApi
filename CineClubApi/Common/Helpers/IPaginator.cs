using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace CineClubApi.Common.Helpers;

public interface IPaginator
{
    Task<List<MovieForListDto>>
        PaginateMoviesList(SearchContainer<SearchMovie> list, int? start, int page, int? end);
}