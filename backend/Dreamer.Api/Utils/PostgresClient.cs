using Dreamer.Shared.Constants;
using Serilog;
using System.Diagnostics;

namespace Dreamer.Api.Utils;

public static class PostgresClient
{
    internal class EnvironmentVariables {
        public required string Host { get; init; }
        public required string Port { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Database { get; init; }

        public static EnvironmentVariables GetVariables()
        {
            Log.Logger.Debug("Getting environment variables for PostgreSQL");
            var errors = new List<string>();

            var host = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredPostgresHost);
            if (String.IsNullOrWhiteSpace(host))
                errors.Add(EnvironmentVariableNames.RequiredPostgresHost);

            var port = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredPostgresPort);
            if (String.IsNullOrWhiteSpace(port))
                errors.Add(EnvironmentVariableNames.RequiredPostgresPort);

            var username = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredPostgresUsername);
            if (String.IsNullOrWhiteSpace(username))
                errors.Add(EnvironmentVariableNames.RequiredPostgresUsername);

            var password = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredPostgresPassword);
            if (String.IsNullOrWhiteSpace(password))
                errors.Add(EnvironmentVariableNames.RequiredPostgresPassword);

            var database = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredPostgresDatabase);
            if (String.IsNullOrWhiteSpace(database))
                errors.Add(EnvironmentVariableNames.RequiredPostgresDatabase);

            if (errors.Count > 0)
            {
                Log.Logger.Error("Missing environment variables for PostgreSQL");
                foreach ( var varName in errors)
                    Console.WriteLine($"{varName}: Cannot be null or empty!");
                Environment.Exit(1);
            }

            Debug.Assert(host != null);
            Debug.Assert(port != null);
            Debug.Assert(username != null);
            Debug.Assert(password != null);
            Debug.Assert(database != null);

            var envVars = new EnvironmentVariables()
            {
                Host = host,
                Port = port,
                Username = username,
                Password = password,
                Database = database
            };

            return envVars;
        }
    }

    public static string GetConnectionString()
    {
        var postgresEnvironmentVariables = EnvironmentVariables.GetVariables();
        var connectionString = $"Host={postgresEnvironmentVariables.Host}; Port={postgresEnvironmentVariables.Port}; Database={postgresEnvironmentVariables.Database}; Username={postgresEnvironmentVariables.Username}; Password={postgresEnvironmentVariables.Password}";
        return connectionString;
    }
}