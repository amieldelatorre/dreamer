namespace Dreamer.Shared.Constants;

public static class EnvironmentVariableNames
{
    public const string RequiredPostgresHost = "DREAMER_POSTGRES_HOST";
    public const string RequiredPostgresPort = "DREAMER_POSTGRES_PORT";
    public const string RequiredPostgresUsername = "DREAMER_POSTGRES_USERNAME";
    public const string RequiredPostgresPassword = "DREAMER_POSTGRES_PASSWORD";
    public const string RequiredPostgresDatabase = "DREAMER_POSTGRES_DATABASE";

    public const string RequiredRedisConnectionString = "DREAMER_REDIS_CONNECTION_STRING";

    public const string RequiredUnleashApiUrl = "UNLEASH_API_URL";
    public const string RequiredUnleashApiKey = "UNLEASH_API_KEY";
    public const string RequiredUnleashAppName = "UNLEASH_APP_NAME";

    public const string JwtValidIssuer = "JWT_VALID_ISSUER";
    public const string JwtValidAudience = "JWT_VALID_AUDIENCE";
    public const string JwtSigningKey = "JWT_SIGNING_KEY";
}