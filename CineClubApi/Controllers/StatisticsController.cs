using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Services.TMDBLibService.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class StatisticsController : CineClubControllerBase
{

    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("top_grossing")]
    public async Task<List<MovieForListDto>> GetTopGrossingMovies()
    {
        return await _statisticsService.TopGrossingMoviesOfAllTime();
    }

}