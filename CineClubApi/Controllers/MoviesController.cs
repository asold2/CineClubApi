﻿using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
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
    public async Task<List<MovieForListDto>> GetMovieByKeywordAsync([FromRoute] string keyword )
    {
        return await _itmdbMovieService.GetMoviesByKeyword(keyword);
    }

    [HttpGet("movie/{id}")]
    public async Task<DetailedMovieDto> GetMovieByIdAsync([FromRoute]int id)
    {
        return await _itmdbMovieService.getMovieById(id);
    }

    [HttpGet("user/lists")]
    public async Task<List<UpdateListDto>> GetUsersListsMovieBelongsTo([FromQuery]Guid userId, [FromQuery] int tmdbId)
    {
        var result = await _movieService.GetUsersListsToWhichMovieBelongs(userId, tmdbId);

        return result;
    }
    
    [HttpGet("movie/lists")]
    public async Task<List<UpdateListDto>> GetAllListsMovieBelongsTo( [FromQuery] int tmdbId)
    {
        var result = await _movieService.GetAllListsMoviebelongsTo(tmdbId);

        return result;
    }

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


    // [HttpGet("movie/image/{url}")]
    // public async Task<byte[]>GetMovieByIdAsync([FromRoute]string url)
    // {
    //     return await _itmdbMovieService.GetMovieImage(url);
    // }
}