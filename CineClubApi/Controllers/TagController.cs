using CineClubApi.Common.DTOs.Tag;
using CineClubApi.Common.Permissions;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.TagResults;
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
    public async Task<ActionResult<CreatedTagResult>> AddTag([FromBody] TagBody tag)
    {
        var result = await _tagService.CreateTag( tag.UserId, tag.Name);

        if (result.StatusCode==200)
        {
            return Ok(result);
        }
        
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


    [HttpGet("tag/my_lists")]
    public async Task<List<TagDto>> GetTagsForMyLists([FromQuery] Guid userId)
    {
        var result = await _tagService.GetTagsForUsersLists(userId);

        if (result==null)
        {
            return new List<TagDto>();
        }
        
        return result;

    }


}