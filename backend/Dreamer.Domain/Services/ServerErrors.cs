using Dreamer.Domain.DTOs;
using Serilog;

namespace Dreamer.Domain.Services
{
    public static class ServerErrors<T>
    {
        public static Result<T> GetInternalServerErrorResult(Result<T> result)
        {
            result.RequestResultStatus = Shared.Constants.RequestResultStatusTypes.ServerError;
            result.AddError("Server", "Cannot handle your request right now, please try again later");
            return result;
        }

        public static void LogException(ILogger logger, string featureName, Exception ex)
        {
            var errorType = ex.InnerException == null ? ex.GetType().ToString() : ex.InnerException.GetType().ToString();
            var errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            logger.Error("{featureName}: could not be processed, {errorType}, {error}", featureName, errorType, errorMessage);
        }
    }
}
