using Dreamer.Shared.Constants;
using Serilog;
using System.Diagnostics;
using StackExchange.Redis;

namespace Dreamer.Api.Utils;

public static class RedisClient
{
    internal class EnvironmentVariables
    {
        public required string ConnectionString { get; init; }

        public static EnvironmentVariables GetVariables()
        {
            Log.Logger.Debug("Getting environment variables for Redis");
            var errors = new List<string>();

            var connectionString = Environment.GetEnvironmentVariable(RequiredEnvironmentVariableNames.RedisConnectionString);
            if (String.IsNullOrWhiteSpace(connectionString))
                errors.Add(RequiredEnvironmentVariableNames.RedisConnectionString);


            if (errors.Count > 0)
            {
                Log.Logger.Error("Missing environment variables for Redis");
                foreach (var varName in errors)
                    Console.WriteLine($"{varName}: Cannot be null or empty!");
                Environment.Exit(1);
            }

            Debug.Assert(connectionString != null);
            var envVars = new EnvironmentVariables()
            {
                ConnectionString = connectionString
            };

            return envVars;
        }
    }

    public static IDatabase GetClient()
    {
        var redisEnvironmentVariables = EnvironmentVariables.GetVariables();
// ConnectionMultiplexer.Connect initiates the connection to the database
// Sufficient to check if redis is available
        var multiplexer = ConnectionMultiplexer.Connect(redisEnvironmentVariables.ConnectionString);
        var redisDatabaseCache = multiplexer.GetDatabase();
        return redisDatabaseCache;
    }
}