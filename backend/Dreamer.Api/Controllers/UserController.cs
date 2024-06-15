using Dreamer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Dreamer.Api.Filters;
using Dreamer.Domain.DTOs;
using Dreamer.Domain.Validators;
using Dreamer.Shared.Constants;
using FluentValidation;

namespace Dreamer.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class UserController(
        IUserService userService,
        IValidator<UserCreate> userCreateValidator,
        Serilog.ILogger logger) : ControllerBase
    {
        [HttpPost]
        // Using a TypeFilter instead of a ServiceFilter because we need to
        //      pass an argument to the FeatureToggleFilter
        // A ServiceFilter needs to be instantiated in Program.cs for the DI container,
        //      so we lose flexibility with passing the feature name
        [TypeFilter(typeof(FeatureToggleFilter), Arguments = [ FeatureName.UserCreate])]
        public async Task<ActionResult<Result<UserView>>> PostUser(UserCreate userCreateObj)
        {
            var validationResult = await userCreateValidator.ValidateAsync(userCreateObj);
            if (!validationResult.IsValid)
            {
                var validationErrorResult = HandleRequestResult<UserView>.GetValidationErrorResult(validationResult);
                return BadRequest(validationErrorResult);
            }

            var result = await userService.Create(userCreateObj);
            logger.Debug("{feature}: returning status of {resultStatus}",
                FeatureName.UserCreate,
                result.RequestResultStatus);
            return HandleRequestResult<UserView>.GetRequestResultAsHttpResponse(result, logger);
        }

        [HttpGet("{userId:Guid}")]
        [Produces("application/json")]
        [TypeFilter(typeof(FeatureToggleFilter), Arguments = [ FeatureName.UserGet])]
        public async Task<Result<UserView>> GetUser(Guid userId)
        {
            Result<UserView> result = new Result<UserView>();
            return result;
        }
    }
}
