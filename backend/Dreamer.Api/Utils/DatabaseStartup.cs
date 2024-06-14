using Dreamer.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Dreamer.Api.Utils
{
    public class DatabaseStartup(string connectionString)
    {
        public bool Configure()
        {
            Log.Logger.Information("Starting configuration of database");
            return IsAvailable() && CreateInitialDatabase() && Migrate();
        }

        private bool IsAvailable()
        {
            Log.Logger.Debug("Checking if the database is available");
            var optionBuilder = new DbContextOptionsBuilder<DreamerDbContext>();
            optionBuilder.UseNpgsql(connectionString);
            var context = new DreamerDbContext(optionBuilder.Options);

            try
            {
                var isAvailable = context.Database.CanConnect();
                Log.Logger.Information("Database connection established: {isAvailable}", isAvailable);
                context.Database.CloseConnection();

                return isAvailable;
            }
            catch 
            {
                Log.Logger.Error("Unable to connect to database, exiting");
                return false;
            }
        }

        private bool CreateInitialDatabase()
        {
            Log.Logger.Debug("Checking if the database tables have been created");
            var optionBuilder = new DbContextOptionsBuilder<DreamerDbContext>();
            optionBuilder.UseNpgsql(connectionString);
            var context = new DreamerDbContext(optionBuilder.Options);

            bool databaseCreated = context.Database.EnsureCreated();
            if (databaseCreated)
                Log.Logger.Information("Initial database tables created");
            context.Database.CloseConnection();
            return true;
        }

        private bool Migrate()
        {
            Log.Logger.Debug("Checking if any migrations are required");
            var migrateDatabaseEnvironmentVariableName = "DREAMER_DATABASE_MIGRATE";

            var optionBuilder = new DbContextOptionsBuilder<DreamerDbContext>();
            optionBuilder.UseNpgsql(connectionString);
            var context = new DreamerDbContext(optionBuilder.Options);

            var pendingMigrationsPresent = context.Database.GetPendingMigrations().Any();
            if (!pendingMigrationsPresent)
            {
                Log.Logger.Information("No migrations needed.");
                context.Database.CloseConnection();
                return true;
            }

            bool migrateDatabase;

            // Check if there are any pending database migrations and check the environment variable"MIGRATE_DATABASE".
            // If there are pending migrations and MIGRATE_DATABASE is true perform the migrations, else exit.
            var migrateDatabaseEnvironmentVariableValue = Environment.GetEnvironmentVariable(migrateDatabaseEnvironmentVariableName);
            if (String.IsNullOrWhiteSpace(migrateDatabaseEnvironmentVariableValue))
                migrateDatabase = false;
            else
                _ = bool.TryParse(migrateDatabaseEnvironmentVariableValue, out migrateDatabase);

            if (!migrateDatabase)
            {
                Log.Logger.Error("There are pending Database Migrations, please set the environment variable '{migrateDatabaseEnvironmentVariableName}=True'", migrateDatabaseEnvironmentVariableName);
                context.Database.CloseConnection();
                return false;
            }

            try
            {
                Log.Logger.Information("{migrateDatabaseEnvironmentVariableName} is 'true', performing database migration", migrateDatabaseEnvironmentVariableName);
                context.Database.Migrate();
                Log.Logger.Information("Database migration performed successfully");
                context.Database.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error performing database migration. Exception: {error}", ex.Message);
                context.Database.CloseConnection();
                return false;
            }
        }
    }
}
