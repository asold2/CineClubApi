using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Services.LikeService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class LikeController : CineClubControllerBase
{

    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [LoggedInPermission]

    [HttpPost("like")]
    public async Task<ActionResult> LikeList([FromBody]UserLikeListBody like)
    {
        var result = await _likeService.UserLikeList(like.UserId, like.ListId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }
    [LoggedInPermission]
    [HttpDelete("unlike")]
    public async Task<ActionResult> UnlikeList([FromBody] UserLikeListBody like)
    {
        var result = await _likeService.UserUnlikeList(like.UserId, like.ListId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
        
    }



}