using Dreamer.Domain.DTOs;

namespace Dreamer.Domain.Services;

public interface IJwtService
{
    bool IsCorrectPassword(string givenPassword, string storedPassword);
    Task<Result<JwtCreateView>> Create(UserLoginCredentialsDto loginCredentialsj);
}
