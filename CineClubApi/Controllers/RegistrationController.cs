using CineClubApi.Common.DTOs.Auth;
using CineClubApi.Common.DTOs.User;
using CineClubApi.Common.RequestBody;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineClubApi.Controllers;

public class RegistrationController : CineClubControllerBase
{
    private readonly IUserService _userService;

    public RegistrationController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("registration")]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResult>> RegisterNewUserAsync([FromBody] CreateAccountRequestBody createAccountRequestBody)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result =
            await _userService.CreateAccount(createAccountRequestBody.AccountDto, createAccountRequestBody.UserDto);
        
        return new ContentResult
        {
            Content = result.Result,
            ContentType = "text/plain",
            StatusCode = result.StatusCode
        };

    }

}