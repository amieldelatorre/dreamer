using Serilog.Events;

namespace Dreamer.Api.Utils
{
    public static class LogLevel
    {
        public static LogEventLevel Get()
        {
            var logLevelEnvironmentVariableName = "LOG_LEVEL";
            var logLevelEnvironmentVariableValue = Environment.GetEnvironmentVariable(logLevelEnvironmentVariableName);

            if (logLevelEnvironmentVariableValue == null)
                return LogEventLevel.Information;

            if (Enum.TryParse<LogEventLevel>(logLevelEnvironmentVariableValue, out LogEventLevel logLevel))
                return logLevel;

            return LogEventLevel.Information;
        }
    }
}
