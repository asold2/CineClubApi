using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace CineClubApi.Common.Helpers;

public interface IPaginator
{
    Task<List<MovieForListDto>>
        PaginateMoviesList(SearchContainer<SearchMovie> list, int? start, int page, int? end);
    
    Task<List<ListDto>>
        PaginateLists(List<ListDto> list, int? page, int? start, int? end);

    // Task<List<UpdateListDto>> PaginateUpdatedListDto(List<UpdateListDto> lists, int page, int start, int end);

    Task<PaginatedResult<UpdateListDto>>
        PaginateUpdatedListDto(List<UpdateListDto> lists, int page, int start, int end);

    
    Task<PaginatedResult<List<MovieForListDto>>>
        PaginateListOfMovieDtos(List<MovieForListDto> lists, int page, int start, int end);
    
}