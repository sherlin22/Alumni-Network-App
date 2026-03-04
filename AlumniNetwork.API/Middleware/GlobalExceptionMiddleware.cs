using AlumniNetwork.Application.Common;
using System.Net;
using System.Text.Json;

namespace AlumniNetwork.API.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail("Unexpected error occurred");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
