namespace BasicConnectApi.Custom;

using BasicConnectApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public sealed class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context is null)
            return;
        if (context.HttpContext.User.Identity is null)
            return;

        context.Result = new UnauthorizedObjectResult(new BaseResponse(false, "Invalid token. Please log in again."));
    }
}