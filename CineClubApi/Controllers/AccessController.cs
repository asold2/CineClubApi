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
    public async Task<ActionResult<SuccessfulLoginResult>> AuthenticateUser([FromBody] AccountDto accountDto)
    {
        var result =  await _userService.AuthenticateUser(accountDto);

        if (result.StatusCode == 400)
        {
            if (result.Result == "Username not found!")
            {
                return new ContentResult
                {
                    Content = "User not found!",
                    ContentType = "text/plain",
                    StatusCode = result.StatusCode
                };
            }
            
            if (result.Result == "Wrong Password!")
            {
                return new ContentResult
                {
                    Content = "Wrong password!",
                    ContentType = "text/plain",
                    StatusCode = result.StatusCode
                };
            }
        }

        var neededUser = await _userService.GetUserId(result.Result);

        var success = new SuccessfulLoginResult
        {
            Token = result.Result,
            UserId = neededUser,
            Result = "Successful Login!"
        };

        return Ok(success);
        

        
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutUser([FromBody] TokenBody tokenBody)
    {
        
        await _userService.LogoutUser(tokenBody);
        return Ok();
    }

    [HttpGet("userId")]
    public async Task<Guid> GetUserIdByRefreshToken([FromQuery] string refreshToken)
    {
        return  await _userService.GetUserId(refreshToken);
    }

}