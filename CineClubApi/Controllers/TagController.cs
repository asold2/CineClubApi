using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Models;
using CineClubApi.Services.ListTagService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class TagController : CineClubControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // [LoggedInPermission]
    [HttpPost("tag")]
    public async Task<ActionResult> AddTag([FromBody] TagBody tag)
    {
        var result = await _tagService.AddTag(tag.ListId, tag.UserId, tag.Name);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }

}