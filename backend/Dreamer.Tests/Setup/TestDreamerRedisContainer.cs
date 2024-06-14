using Npgsql;
using Testcontainers.Redis;

namespace Tests.Setup
{
    public class TestDreamerRedisContainer
    {
        public readonly RedisContainer Container = new RedisBuilder()
            .WithImage("redis:7.2.4")
            .Build();
        public string ConnectionString { get; set; } = string.Empty;

        public async Task BuildContainer()
        {
            await Container.StartAsync();
            ConnectionString = Container.GetConnectionString();
        }
    }
}
