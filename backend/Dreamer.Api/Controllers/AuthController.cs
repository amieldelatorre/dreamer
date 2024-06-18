using Dreamer.Api.Filters;
using Dreamer.Domain.DTOs;
using Dreamer.Domain.Services;
using Dreamer.Shared.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Dreamer.Api.Controllers;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class AuthController(
    IValidator<UserLoginCredentialsDto> userLoginCredentialsValidator,
    IJwtService jwtService,
    Serilog.ILogger logger
    ) : Controller
{
    [HttpPost("login")]
    [TypeFilter(typeof(FeatureToggleFilter), Arguments = [FeatureName.Login])]
    public async Task<ActionResult<Result<JwtCreateView>>> Login(UserLoginCredentialsDto loginCredentials)
    {
        var validationResult = await userLoginCredentialsValidator.ValidateAsync(loginCredentials);
        if (!validationResult.IsValid)
        {
            var validationErrorResult = HandleRequestResult<JwtCreateView>.GetValidationErrorResult(validationResult);
            return BadRequest(validationErrorResult);
        }

        var result = await jwtService.Create(loginCredentials);
        logger.Debug("{feature}: returning status of {resultStatus}",
                FeatureName.Login,
                result.RequestResultStatus);

        return HandleRequestResult<JwtCreateView>.GetRequestResultAsHttpResponse(result, logger);
    }
}
