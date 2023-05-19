using CineClubApi.Services.AccountService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CineClubApi.Common.Permissions;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LoggedInPermission : Attribute, IAuthorizationFilter
{

    private readonly IAuthService _authService;

    public LoggedInPermission(IAuthService authService)
    {
        _authService = authService;
    }

    public LoggedInPermission()
    {
        
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

        string authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new UnauthorizedResult(); // Return 401 Unauthorized
            return;
        }

        // Check if the authorization header contains a bearer token
        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult(); // Return 401 Unauthorized
            return;
        }
        
        // Extract the token from the authorization header
        string token = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Perform token validation and authorization checks
        bool isAuthorized = authService.ValidateToken(token);

        if (!isAuthorized)
        {
            context.Result = new ForbidResult(); // Return 403 Forbidden
            return;
        }

    }
}