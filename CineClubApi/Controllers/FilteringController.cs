using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.RequestBody;
using CineClubApi.Services.TMDBLibService.FilteredLists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CineClubApi.Controllers;

public class FilteringController : CineClubControllerBase
{

    private IFilteredListService _filteredListService;

    public FilteringController(IFilteredListService filteredListService)
    {
        _filteredListService = filteredListService;
    }
    
    
    [HttpGet("filter")]
    public async Task<PaginatedListOfMovies> GetFilteredListOfMovies([FromQuery]int page, [FromQuery]int? start, [FromQuery] int? end, [FromQuery] FilteringRequestBody body)
    {
        return await _filteredListService.GetFilteredListOfMovies(page, start, end,
            body.GenreIds,
            body.Year,
            body.ReleasedAfter,
            body.ReleasedBefore,
            body.LeastVoteAverage,
            body.Language,
            body.sortBy,
            body.IncludeAdultMovies
        );
    }

}