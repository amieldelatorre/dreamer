using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace Dreamer.Api.Filters;

public class ExceptionHandlerFilter(ILogger logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.Error("Exception has occured: {exception}", context.Exception.ToString());

        // return an empty Internal Server Error when an unhandled exception is thrown
        context.Result =  new ObjectResult(null)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}