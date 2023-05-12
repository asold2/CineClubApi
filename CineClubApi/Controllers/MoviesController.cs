using CineClubApi.Common.DTOs.Movies;
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


    [HttpGet("movies/{keyword}")]
    public async Task<List<MovieForListDto>> GetMovieByKeywordAsync([FromRoute] string keyword )
    {
        return await _tmdbLibService.GetMoviesByKeyword(keyword);
    }

    [HttpGet("movie/{id}")]
    public async Task<DetailedMovieDto> GetMovieByIdAsync([FromRoute]int id)
    {
        return await _tmdbLibService.getMovieById(id);
    }
    
    
    [HttpGet("movie/image/{url}")]
    public async Task<byte[]>GetMovieByIdAsync([FromRoute]string url)
    {
        return await _tmdbLibService.GetMovieImage(url);
    }
}