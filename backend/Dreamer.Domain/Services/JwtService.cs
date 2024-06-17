using Dreamer.Domain.DTOs;

namespace Dreamer.Domain.Services;

public class JwtService : IJwtService
{
    public bool IsCorrectPassword(string givenPassword, string storedPassword)
    {
        throw new NotImplementedException();
        return true;
    }

    public Task<Result<JwtCreateView>> Create(UserLoginCredentialsDto loginCredentials)
    {
        throw new NotImplementedException();
    }
}
