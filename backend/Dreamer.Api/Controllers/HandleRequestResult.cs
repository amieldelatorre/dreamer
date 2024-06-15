using Dreamer.Shared.Constants;
using Dreamer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Dreamer.Api.Controllers;

public static class HandleRequestResult<T>
{
    public static ActionResult GetRequestResultAsHttpResponse(
        Result<T> result,
        ILogger logger)
    {
        switch(result.RequestResultStatus)
        {
            case RequestResultStatusTypes.Success:
                return new OkObjectResult(result);
            case RequestResultStatusTypes.Created:
                return new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            case RequestResultStatusTypes.UserError:
                return new BadRequestObjectResult(result);
            case RequestResultStatusTypes.ServerError:
                return new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            default:
                logger.Error("{feature}: returning unexpected request status {status}, will return {internalServerErrorInstead}",
                    FeatureName.UserCreate,
                    result.RequestResultStatus,
                    StatusCodes.Status500InternalServerError);
                return new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
        }
    }

    public static Dictionary<string, List<string>> ResultErrorDict(IEnumerable<KeyValuePair<string, string[]>> errs)
    {
        var errorResultDict = new Dictionary<string, List<string>>();

        foreach (var item in errs)
        {
            errorResultDict[item.Key] = item.Value.ToList();
        }

        return errorResultDict;
    }
}