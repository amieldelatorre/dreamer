using Dreamer.Api.Filters;
using Dreamer.Domain.DTOs;
using Dreamer.Domain.Services;
using Dreamer.Shared.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Dreamer.Api.Controllers;

[Route("api/v1/auth/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class JwtController(
    IValidator<UserLoginCredentialsDto> userLoginCredentialsValidator,
    IJwtService jwtService,
    Serilog.ILogger logger
    ) : Controller
{
    [TypeFilter(typeof(FeatureToggleFilter), Arguments = [FeatureName.JwtCreate])]
    public async Task<IActionResult> PostJwt(UserLoginCredentialsDto loginCredentials)
    {
        var validationResult = await userLoginCredentialsValidator.ValidateAsync(loginCredentials);
        if (!validationResult.IsValid)
        {
            var validationErrorResult = HandleRequestResult<JwtCreateView>.GetValidationErrorResult(validationResult);
            return BadRequest(validationErrorResult);
        }
        return Ok();
    }
}
