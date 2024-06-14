using Testcontainers.PostgreSql;
using Npgsql;


namespace Tests.Setup
{
    
    public class TestDreamerPostgresContainer
    {
        public PostgreSqlContainer Container { get; set; } = new PostgreSqlBuilder()
            .WithDatabase("dreamer")
            .WithUsername("root")
            .WithPassword("root")
            .Build();
        public string ConnectionString {  get; set; } = string.Empty;

        /// <summary>
        /// Class <c>TestDreamerPostgresContainer</c> is a PostgreSQL container loaded with test data
        /// </summary>
        public async Task BuildContainer()
        {
            await Container.StartAsync();
            ConnectionString = Container.GetConnectionString();

            NpgsqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            var sqlScript = await File.ReadAllTextAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "dreamer_postgres_dump.sql"));
            var postgresCommand = new NpgsqlCommand(sqlScript, connection);
            await postgresCommand.ExecuteNonQueryAsync();
            await postgresCommand.DisposeAsync();
            await connection.CloseAsync();
        }
    }
}
