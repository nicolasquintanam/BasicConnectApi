namespace BasicConnectApi.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BasicConnectApi.Models;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage)
                                                  .ToList();
            context.Result = new UnprocessableEntityObjectResult(new BaseResponse(false, errors.FirstOrDefault()));
        }

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}