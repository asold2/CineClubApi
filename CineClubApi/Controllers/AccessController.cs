using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.LoginResult;
using CineClubApi.Models.Auth;
using CineClubApi.Services.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class AccessController : CineClubControllerBase
{

    private readonly IUserService _userService;

    public AccessController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("token")]
    public async Task<ActionResult<ServiceResult>> AuthenticateUser([FromBody] AccountDto accountDto)
    {
        var result =  await _userService.AuthenticateUser(accountDto);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };
        
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutUser([FromBody] TokenBody tokenBody)
    {
        
        await _userService.LogoutUser(tokenBody);
        return Ok();
    }

}