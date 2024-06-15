using Dreamer.Domain.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace Dreamer.Api.Middlewares;

public static class GlobalExceptionHandlerMiddleware
{
    public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}