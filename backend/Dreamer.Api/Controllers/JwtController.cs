using Dreamer.Domain.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Dreamer.Api.Controllers;

[Route("api/v1/auth/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class JwtController(
    IValidator<UserLoginCredentialsDto> userLoginCredentialsValidator,
    Serilog.ILogger logger
    ) : Controller
{
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
