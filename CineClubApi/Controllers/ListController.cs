using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Services.ListService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CineClubApi.Controllers;

public class ListController : CineClubControllerBase
{
    private readonly IListService _listService;

    public ListController(IListService listService)
    {
        _listService = listService;
    }

    [HttpPost("list")]
    public async Task<ActionResult<ServiceResult>> CreateNamedList([FromBody] ListDto listDto)
    {
        var result = await _listService.CreateNamedList(listDto);

        return Ok(result);
    }

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

    [HttpGet("lists")]
    public async Task<ActionResult<IList<UpdateListDto>>> GetListsByUserdId([FromQuery] string tokenBody)
    {
        return Ok(await _listService.GetListsByUserId(tokenBody));
    }


    [HttpDelete("list")]
    public async Task<ActionResult> DeleteListById([FromQuery] Guid listId)
    {
        var result = await _listService.DeleteListById(listId);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };

    }
}