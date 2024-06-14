using DotNet.Testcontainers.Networks;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Tests.Setup
{
    public class TestUnleashPostgresContainer
    {
        public PostgreSqlContainer Container { get; set; } 
        public string ConnectionString { get; set; } = string.Empty;

        public TestUnleashPostgresContainer(INetwork network)
        {
            Container = new PostgreSqlBuilder()
                .WithDatabase("unleash")
                .WithUsername("root")
                .WithPassword("root")
                .WithNetwork(network) 
                .Build();
        }

        /// <summary>
        /// Class <c>TestUnleashPostgresContainer</c> is a PostgreSQL container loaded with test data
        /// </summary>
        public async Task BuildContainer()
        {
            await Container.StartAsync();
            ConnectionString = Container.GetConnectionString();

            NpgsqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            var sqlScript = await File.ReadAllTextAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "unleash_postgres_dump.sql"));
            var postgresCommand = new NpgsqlCommand(sqlScript, connection);
            await postgresCommand.ExecuteNonQueryAsync();
            await postgresCommand.DisposeAsync();
            await connection.CloseAsync();
        }
    }
}
