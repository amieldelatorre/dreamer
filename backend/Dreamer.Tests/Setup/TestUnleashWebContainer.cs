using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

namespace Tests.Setup
{
    public class TestUnleashWebContainer
    {
        private const int ContainerPort = 4242;
        public DotNet.Testcontainers.Containers.IContainer Container { get; set; }
        public Uri? Endpoint;

        public TestUnleashWebContainer(string host, INetwork network) 
        {
            var databaseUrl = $"postgres://root:root@{host}:5432/unleash";

            Container = new ContainerBuilder()
                .WithImage("unleashorg/unleash-server:latest")
                .WithPortBinding(ContainerPort, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(ContainerPort)))
                .WithEnvironment("DATABASE_URL", databaseUrl)
                .WithEnvironment("DATABASE_SSL", "false")
                .WithNetwork(network)
                .Build();
        }


        public async Task BuildContainer(CancellationToken cancellationToken)
        {
            await Container.StartAsync(cancellationToken);
            var requestUri = new UriBuilder(Uri.UriSchemeHttp, Container.Hostname, Container.GetMappedPublicPort(ContainerPort), "api").Uri;

            Endpoint = requestUri;
        }
    }
}
