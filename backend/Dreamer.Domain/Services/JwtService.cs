using Dreamer.Cache;
using Dreamer.DataAccess.Models;
using Dreamer.Domain.DTOs;
using Dreamer.Shared.Constants;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Dreamer.Domain.Services;

public class JwtService(
    IJwtCache jwtCache,
    IUserCache userCache, 
    JwtEnvironmentVariables jwtEnvironmentVariables,
    Serilog.ILogger logger) : IJwtService
{
    private const int TokenValidityLengthDays = 7;

    public bool IsCorrectPassword(string givenPassword, string storedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(givenPassword, storedPassword);
    }

    public async Task<Result<JwtCreateView>> Create(UserLoginCredentialsDto loginCredentials)
    {
        logger.Debug("{featureName} starting", FeatureName.Login);
        var result = new Result<JwtCreateView>();

        var user = await userCache.GetUserByEmail(loginCredentials.Email);
        if (user == null || !IsCorrectPassword(loginCredentials.Password, user.Password))
        {
            return GetInvalidCredentialsResult(result);
        }

        var newJwtExpiryDate = DateTime.UtcNow.AddDays(TokenValidityLengthDays);
        var dateTimeNow = DateTime.UtcNow;

        var newJwt = new Jwt()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpiryDate = newJwtExpiryDate,
            DateCreated = dateTimeNow,
            DateModified = dateTimeNow,
            IsDisabled = false,
            DateDisabled = null
        };

        await jwtCache.Create(newJwt);
        var accessToken = CreateJwtAccessToken(newJwt);

        logger.Debug("{featureName}: processed with new Id '{jwtId}'.",
                FeatureName.Login,
                newJwt.Id);
        result.RequestResultStatus = RequestResultStatusTypes.Created;
        result.Item = JwtCreateViewFromJwtAndAccessToken(accessToken, newJwt);

        return result;
    }

    private static Result<JwtCreateView> GetInvalidCredentialsResult(Result<JwtCreateView> result)
    {
        result.AddError("Credentials", "Invalid Email and Password combination");
        result.RequestResultStatus = RequestResultStatusTypes.UserError;
        return result;
    }

    private string CreateJwtAccessToken(Jwt jwt)
    {
        var claims = new Dictionary<string, object>()
        {
            [ClaimTypes.Email] = jwt.User.Email,
            ["jwtId"] = jwt.Id
        };

        // TODO: Set these token validation parameters values up properly
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtEnvironmentVariables.SigningKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtEnvironmentVariables.ValidIssuer,
            Audience = jwtEnvironmentVariables.ValidAudience,
            Claims = claims,
            IssuedAt = jwt.DateCreated,
            NotBefore = jwt.DateCreated,
            Expires = jwt.ExpiryDate,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JsonWebTokenHandler();
        var accessTokenString = tokenHandler.CreateToken(descriptor);

        return accessTokenString;
    }

    private static JwtCreateView JwtCreateViewFromJwtAndAccessToken(string accessToken, Jwt jwt)
    {
        return new JwtCreateView()
        {
            Id = jwt.Id,
            UserId = jwt.User.Id,
            AccessToken = accessToken,
            ExpiryDate = jwt.ExpiryDate,
            DateCreated = jwt.DateCreated,
            DateModified = jwt.DateModified,
        };
    }
}
