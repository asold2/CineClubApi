using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
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

    [LoggedInPermission]
    [HttpPost("tag")]
    public async Task<ActionResult> AddTag([FromBody] TagBody tag)
    {
        var result = await _tagService.CreateTag( tag.UserId, tag.Name);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode,
            
            
        };
    }
    
    [LoggedInPermission]
    [HttpPost("tag/list")]
    public async Task<ActionResult> AssignTagToList([FromBody]AssignTagToListBody tag)
    {
        var result = await _tagService.AssignTagToList(tag.TagId, tag.ListId, tag.UserId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }


    [HttpGet("tags")]
    public async Task<List<TagForListDto>> GetTags()
    {
        var result = await _tagService.GetAllTags();

        return result;
    }

    [LoggedInPermission]
    [HttpDelete("tag")]
    public async Task<ActionResult> DeleteTag([FromBody] DeleteTagBody tagBody)
    {
        var result = await _tagService.DeleteTag(tagBody.TagId, tagBody.UserId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
    }

    [HttpGet("tag/{tagId}")]
    public async Task<TagDto> GetTagById([FromRoute] Guid tagId)
    {
        var result = await _tagService.GetTag(tagId);

        return result;
    }




}