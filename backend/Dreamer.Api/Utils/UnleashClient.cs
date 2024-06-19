using Dreamer.Shared.Constants;
using Serilog;
using System.Diagnostics;
using Unleash;
using Unleash.ClientFactory;

namespace Dreamer.Api.Utils;

public static class UnleashClient
{
    internal class EnvironmentVariables
    {
        public required string ApiUrl { get; init; }
        public required string ApiKey { get; init; }
        public required string AppName { get; init; }

        public static EnvironmentVariables GetVariables()
        {
            Log.Logger.Debug("Getting environment variables for Unleash");
            var errors = new List<string>();

            var apiUrl = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredUnleashApiUrl);
            if (String.IsNullOrWhiteSpace(apiUrl))
                errors.Add(EnvironmentVariableNames.RequiredUnleashApiUrl);

            var apiKey = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredUnleashApiKey);
            if (String.IsNullOrWhiteSpace(apiKey))
                errors.Add(EnvironmentVariableNames.RequiredUnleashApiKey);

            var appName = Environment.GetEnvironmentVariable(EnvironmentVariableNames.RequiredUnleashAppName);
            if (String.IsNullOrWhiteSpace(appName))
                errors.Add(EnvironmentVariableNames.RequiredUnleashAppName);

            if (errors.Count > 0)
            {
                Log.Logger.Debug("Missing environment variables for Unleash");
                foreach (var varName in errors)
                    Console.WriteLine($"{varName}: Cannot be null or empty!");
                Environment.Exit(1);
            }

            Debug.Assert(apiUrl != null);
            Debug.Assert(apiKey != null);
            Debug.Assert(appName != null);

            var envVars = new EnvironmentVariables()
            {
                ApiUrl = apiUrl,
                ApiKey = apiKey,
                AppName = appName
            };

            return envVars;
        }
    }

    public static async Task<IUnleash> GetClient()
    {
        var unleashEnvironmentVariables = EnvironmentVariables.GetVariables();
        var settings = new UnleashSettings()
        {
            AppName = unleashEnvironmentVariables.AppName,
            UnleashApi = new Uri(unleashEnvironmentVariables.ApiUrl),
            CustomHttpHeaders = new Dictionary<string, string>()
            {
                { "Authorization", unleashEnvironmentVariables.ApiKey },
            }
        };
        var unleashFactory = new UnleashClientFactory();
        // CreateClientAsync initiates the connection to the unleash server
        // Sufficient to check if unleash server is available
        IUnleash unleash = await unleashFactory.CreateClientAsync(settings, synchronousInitialization: true);
        return unleash;
    }
}