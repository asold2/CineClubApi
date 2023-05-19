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
    public async Task<List<MovieForListDto>> GetFilteredListOfMovies([FromQuery] FilteringRequestBody body)
    {
        return await _filteredListService.GetFilteredListOfMovies(
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