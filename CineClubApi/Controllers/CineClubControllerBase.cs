using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CineClubApi.Controllers;


[ApiController]
[Route("cine_club/[controller]")]
public class CineClubControllerBase : Controller
{
    
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (!ModelState.IsValid && context?.Result?.GetType() == typeof(BadRequestObjectResult))
        {
            BadRequestObjectResult result = (BadRequestObjectResult) context.Result;
            object? value = result.Value;
            if (value != null)
            {
                context.Result =
                    new BadRequestObjectResult(((ValidationProblemDetails) value).Errors);
                context.ExceptionHandled = true;
            }
        }

        if (context != null)
        {
            base.OnActionExecuted(context);
        }
    }
}