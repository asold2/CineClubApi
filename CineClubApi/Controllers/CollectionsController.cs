using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Enums;
using CineClubApi.Services.TMDBLibService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class CollectionsController : CineClubControllerBase
{

    private readonly ITMDBLibService _tmdbLibService;

    public CollectionsController(ITMDBLibService tmdbLibService)
    {
        _tmdbLibService = tmdbLibService;
    }

    [HttpGet("collection/popular")]
    public async Task<List<MovieForListDto>> GetPopularMovies()
    {
        return await _tmdbLibService.GetPopularMovies();
    }
    
    [HttpGet("collection/toprated")]
    public async Task<List<MovieForListDto>> GetTopRatedMovies()
    {
        return await _tmdbLibService.GetTopRatedMovies();
    }
    
    [HttpGet("collection/upcoming")]
    public async Task<List<MovieForListDto>> GetUpcomingMovies()
    {
        return await _tmdbLibService.GetUpcomingMovies();
    }
    
    [HttpGet("collection/trending/{period:int}")]
    public async Task<List<MovieForListDto>> GetTrendingMovies([FromRoute] int period)
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
        
        return await _tmdbLibService.GetTrendingMovies(trendingPeriod);
    }
    
    

}