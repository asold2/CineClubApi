using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Services.ListService;
using CineClubApi.Services.MovieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CineClubApi.Controllers;

public class ListController : CineClubControllerBase
{
    private readonly IListService _listService;
    private readonly IMovieService _movieService;

    public ListController(IListService listService, IMovieService movieService)
    {
        _listService = listService;
        _movieService = movieService;
    }

    [LoggedInPermission]
    [HttpPost("list")]
    public async Task<ActionResult<ServiceResult>> CreateNamedList([FromBody] ListDto listDto)
    {
        var result = await _listService.CreateNamedList(listDto);

        return Ok(result);
    }

    [LoggedInPermission]
    [HttpPut("list")]
    public async Task<ActionResult<ServiceResult>> UpdateNamedList([FromBody] UpdateListDto updateListDto)
    {
        var result = await _listService.UpdateListNameOrStatus(updateListDto);

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
        var result = await _listService.DeleteListById(body.ListId, body.UserId);
        
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






}