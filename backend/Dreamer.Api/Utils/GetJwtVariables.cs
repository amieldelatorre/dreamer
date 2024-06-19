using Dreamer.Domain.Services;
using Dreamer.Shared.Constants;
using Serilog;
using System.Diagnostics;

namespace Dreamer.Api.Utils;

public static class GetJwtVariables
{
    private const string ValidIssuerDefault = "dreamer";
    private const string ValidAudienceDefault = "dreamer";

    public static JwtEnvironmentVariables GetVariables()
    {
        Log.Logger.Debug("Getting environment variables for Jwt");
        var errors = new List<string>();

        var validIssuer = Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtValidIssuer);
        if (string.IsNullOrWhiteSpace(validIssuer))
            validIssuer = ValidIssuerDefault;

        var validAudience = Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtValidAudience);
        if (string.IsNullOrWhiteSpace(validAudience))
            validAudience = ValidAudienceDefault;

        var signingKey = Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtSigningKey);
        if (string.IsNullOrWhiteSpace(signingKey))
            errors.Add(EnvironmentVariableNames.JwtSigningKey);

        if (errors.Count > 0)
        {
            Log.Logger.Debug("Missing environment variables for Jwt Authentication");
            foreach (var varName in errors)
                Console.WriteLine($"{varName}: Cannot be null or empty!");
            Environment.Exit(1);
        }

        Debug.Assert(validIssuer != null);
        Debug.Assert(validAudience != null);
        Debug.Assert(signingKey != null);

        var envVars = new JwtEnvironmentVariables()
        {
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            SigningKey = signingKey 
        };

        return envVars;
    }
}
