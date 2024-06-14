namespace Dreamer.Shared.Constants;

public static class RequiredEnvironmentVariableNames
{
    public const string PostgresHost = "DREAMER_POSTGRES_HOST";
    public const string PostgresPort = "DREAMER_POSTGRES_PORT";
    public const string PostgresUsername = "DREAMER_POSTGRES_USERNAME";
    public const string PostgresPassword = "DREAMER_POSTGRES_PASSWORD";
    public const string PostgresDatabase = "DREAMER_POSTGRES_DATABASE";

    public const string RedisConnectionString = "DREAMER_REDIS_CONNECTION_STRING";

    public const string UnleashApiUrl = "UNLEASH_API_URL";
    public const string UnleashApiKey = "UNLEASH_API_KEY";
    public const string UnleashAppName = "UNLEASH_APP_NAME";
}