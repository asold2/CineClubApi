using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Lists;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class CollectionsController : CineClubControllerBase
{

    private readonly ITmdbListService _tmdbListService;

    public CollectionsController(ITmdbListService tmdbLibService)
    {
        _tmdbListService = tmdbLibService;
    }

    [HttpGet("collection/popular")]
    public async Task<List<MovieForListDto>> GetPopularMovies([FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        return await _tmdbListService.GetPopularMovies(page, start, end);
    }
    
    [HttpGet("collection/toprated")]
    public async Task<List<MovieForListDto>> GetTopRatedMovies([FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        return await _tmdbListService.GetTopRatedMovies(page, start, end);
    }
    
    [HttpGet("collection/upcoming")]
    public async Task<List<MovieForListDto>> GetUpcomingMovies([FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        return await _tmdbListService.GetUpcomingMovies(page, start, end);
    }
    
    [HttpGet("collection/trending/{period:int}")]
    public async Task<List<MovieForListDto>> GetTrendingMovies([FromRoute] int period, [FromQuery]int page, [FromQuery]int start, [FromQuery] int end)
    {
        var trendingPeriod = new TimePeriod();

        switch (period)
        {
            case 1:
                trendingPeriod = TimePeriod.Day;
                break;
            case 2:
                trendingPeriod = TimePeriod.Week;
                break;
        }
        
        return await _tmdbListService.GetTrendingMovies(trendingPeriod, page, start, end);
    }
    
    

}