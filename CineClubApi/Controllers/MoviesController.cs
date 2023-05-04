using CineClubApi.Models;
using CineClubApi.Services.TMDBLibService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class MoviesController : CineClubControllerBase
{

    private readonly ITMDBLibService _tmdbLibService;

    public MoviesController(ITMDBLibService tmdbLibService)
    {
        _tmdbLibService = tmdbLibService;
    }
    
    [HttpGet("movie")]
    public async Task GetMovieAsync()
    {
        await _tmdbLibService.getAllMovies();
    }

    [HttpGet("movie")]
    public async Task<MovieDao> GetMovieByKeyword([FromRoute] string keyword)
    {
        return await _tmdbLibService.GetMovieByKeyword(keyword);
    }
}