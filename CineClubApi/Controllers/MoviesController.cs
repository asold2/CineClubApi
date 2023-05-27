using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Permissions;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Services.MovieService;
using CineClubApi.Services.TMDBLibService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class MoviesController : CineClubControllerBase
{

    private readonly ITMDBMovieService _itmdbMovieService;
    private readonly IMovieService _movieService;

    public MoviesController(ITMDBMovieService itmdbMovieService, IMovieService movieService)
    {
        _itmdbMovieService = itmdbMovieService;
        _movieService = movieService;
    }


    [HttpGet("movies/{keyword}")]
    public async Task<List<MovieForListDto>> GetMovieByKeywordAsync([FromRoute] string keyword)
    {
        return await _itmdbMovieService.GetMoviesByKeyword(keyword);
    }

    [HttpGet("movie/{id}")]
    public async Task<DetailedMovieDto> GetMovieByIdAsync([FromRoute] int id)
    {
        return await _itmdbMovieService.getMovieById(id);
    }

    
    
    [HttpGet("user/lists")]
    public async Task<ActionResult<List<SimpleListDto>>> GetUsersListsMovieBelongsTo([FromQuery] Guid userId,
        [FromQuery] int tmdbId)
    {
        var result = await _movieService.GetUsersListsToWhichMovieBelongs(userId, tmdbId);


        if (result == null)
        {
            return Ok(new List<SimpleListDto>());
        }


        return Ok(result);
    }
    [LoggedInPermission]
    [HttpPost("movie/like")]
    public async Task<ActionResult> LikeMovie([FromQuery] Guid userid, [FromQuery] int tmdbId)
    {
        var result = await _movieService.AddMovieToLikedList(userid, tmdbId);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }
    [LoggedInPermission]
    [HttpPost("movie/watched")]
    public async Task<ActionResult> WatchedMovie([FromQuery] Guid userid, [FromQuery] int tmdbId)
    {
        var result = await _movieService.AddMovieToWatchedList(userid, tmdbId);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }

    [LoggedInPermission]
    [HttpDelete("movie/watched")]
    public async Task<ActionResult> RemoveFromWatchedMovie([FromQuery] Guid userid, [FromQuery] int tmdbId)
    {
        var result = await _movieService.RemoveMovieFromWatchedList(userid, tmdbId);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }
    
    
    [LoggedInPermission]
    [HttpDelete("movie/liked")]
    public async Task<ActionResult> RemoveFromLikedMovie([FromQuery] Guid userid, [FromQuery] int tmdbId)
    {
        var result = await _movieService.RemoveMovieFromLikedList(userid, tmdbId);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }
    
    
    
}

    
