namespace Dreamer.Domain.Services;

public interface IJwtService
{
    bool IsCorrectPassword(string givenPassword, string storedPassword);
}
