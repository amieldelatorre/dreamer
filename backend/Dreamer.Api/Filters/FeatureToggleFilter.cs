using Dreamer.Domain.Services;
using Dreamer.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dreamer.Api.Filters
{
    public class FeatureToggleFilter(IFeatureToggleService featureToggleService, string callerName) : Attribute, IAsyncActionFilter
    {
        private const string FeatureToggleGlobalPrefix = "dreamer";

        private string FeatureToggleName(string featureName) => $"{FeatureToggleGlobalPrefix}_{featureName}";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var featureToggleName = FeatureToggleName(callerName);
            var featureEnabled = await featureToggleService.IsFeatureEnabled(featureToggleName);
            if (!featureEnabled)
            {
                context.Result = new NotFoundObjectResult(null);
                return;
            }

            await next();
        }
    }
}
