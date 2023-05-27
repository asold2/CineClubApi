using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Services.ListService;
using CineClubApi.Services.ListService.RecommendedLists;
using CineClubApi.Services.MovieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CineClubApi.Controllers;

public class ListController : CineClubControllerBase
{
    private readonly IMovieService _movieService;
    private IServiceResultListService _serviceResultListService;
    private IRecommendedListsService _recommendedListsService;
    public ListController( IMovieService movieService,
        IRecommendedListsService recommendedListsService,
        IServiceResultListService serviceResultListService)
    {
        _serviceResultListService = serviceResultListService;
        _recommendedListsService = recommendedListsService;
        
        
        _movieService = movieService;
    }

    [LoggedInPermission]
    [HttpPost("list")]
    public async Task<ActionResult<ServiceResult>> CreateNamedList([FromBody] ListDto listDto)
    {
        var result = await _serviceResultListService.CreateNamedList(listDto);

        return Ok(result);
    }

    [LoggedInPermission]
    [HttpPut("list")]
    public async Task<ActionResult<ServiceResult>> UpdateNamedList([FromBody] UpdateListDto updateListDto)
    {
        var result = await _serviceResultListService.UpdateListNameOrStatus(updateListDto);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }



    [LoggedInPermission]
    [HttpDelete("list")]
    public async Task<ActionResult> DeleteListById([FromBody] DeleteListBody body)
    {
        var result = await _serviceResultListService.DeleteListById(body.ListId, body.UserId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };

    }

    [LoggedInPermission]
    [HttpPost("list/movie")]
    public async Task<ActionResult> AddMovieToList([FromBody] AddMovieToListBody body)
    {
        var result = await _movieService.AddMovieToList(body.ListId, body.UserId, body.TmdbId);

        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }
    
    [LoggedInPermission]
    [HttpDelete("list/movie")]
    public async Task<ActionResult> DeleteMovieFromList([FromBody] AddMovieToListBody body)
    {
        var result = await _movieService.DeleteMovieFromList(body.ListId, body.UserId, body.TmdbId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
        
    }

    [LoggedInPermission]
    [HttpGet("list/recommendations")]
    public async Task<PaginatedResult<List<MovieForListDto>>> GetListOfRecommendedMovies([FromQuery] Guid userId, 
        [FromQuery]int page,
        [FromQuery] int start,
        [FromQuery]int end)
    {
        return await _recommendedListsService.GetListOfRecommendedMoviesForUser(userId, page, start, end);
        
    }
    

}