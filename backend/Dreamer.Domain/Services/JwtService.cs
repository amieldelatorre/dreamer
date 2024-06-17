using Dreamer.Cache;
using Dreamer.Domain.DTOs;
using Dreamer.Shared.Constants;

namespace Dreamer.Domain.Services;

public class JwtService(IUserCache userCache, Serilog.ILogger logger) : IJwtService
{
    public bool IsCorrectPassword(string givenPassword, string storedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(givenPassword, storedPassword);
    }

    public async Task<Result<JwtCreateView>> Create(UserLoginCredentialsDto loginCredentials)
    {
        logger.Debug("{featureName} starting", FeatureName.JwtCreate);
        var result = new Result<JwtCreateView>();

        var user = await userCache.GetUserByEmail(loginCredentials.Email);
        if (user == null || !IsCorrectPassword(loginCredentials.Password, user.Password))
        {
            return GetInvalidCredentialsResult(result);
        } 
        throw new NotImplementedException();
    }

    private Result<JwtCreateView> GetInvalidCredentialsResult(Result<JwtCreateView> result)
    {
        result.AddError("Credentials", "Invalid Email and Password combination");
        result.RequestResultStatus = RequestResultStatusTypes.UserError;
        return result;
    }
}
