using Serilog;

namespace Tests.Setup
{
    public static class SerilogLogger
    {
        public static ILogger GetLogger()
        {
            // Very base logger, don't need outputs for this one
            var logger = new LoggerConfiguration()
                .MinimumLevel.Fatal()
                .CreateLogger();
            return logger;
        }
    }
}
