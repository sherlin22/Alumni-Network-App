using AlumniNetwork.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlumniNetwork.API.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
            .ToArray();

        context.Result = new BadRequestObjectResult(ApiResponse<string[]>.Fail(string.Join("; ", errors)));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
