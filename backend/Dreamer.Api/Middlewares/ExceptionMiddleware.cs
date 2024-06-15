using Dreamer.Domain.DTOs;
using Newtonsoft.Json;

namespace Dreamer.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.Error("Unhandled exception occured: {exception}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var result = new Result<object>();
        result.AddError("Server", "Cannot handle your request right now. Please try again later.");
        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}