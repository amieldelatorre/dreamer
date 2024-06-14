using Dreamer.DataAccess.Repository;
using Dreamer.Shared.Constants;
using Dreamer.Domain.Services;
using StackExchange.Redis;
using Serilog;
using Tests.Setup;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Unleash.ClientFactory;
using Unleash;
using Microsoft.VisualBasic;
using Dreamer.Api.Filters;

namespace Tests.FilterTests
{
    public class FeatureToggleFilterTest
    {
        private TestDreamerRedisContainer _redisContainer;
        private TestUnleashPostgresContainer _unleashPostgresContainer;
        private TestUnleashWebContainer _unleashWebContainer;
        private INetwork _network;
        private IDatabaseCacheRepository _databaseCacheRepository;
        private IFeatureToggleService _featureToggleService;
        private readonly ILogger _logger = SerilogLogger.GetLogger();
        private const string UnleashAppName = "dreamer";
        private const string UnleashApiKey = "*:production.dc7968908207aee9c8bbb6866956060be73c7de60bdfc6e5f69b983c";


        [OneTimeSetUp]
        public async Task Setup()
        {
            _network = new NetworkBuilder()
              .WithName(Guid.NewGuid().ToString("D"))
              .Build();

            _redisContainer = new TestDreamerRedisContainer();
            await _redisContainer.BuildContainer();
            var multiplexer = ConnectionMultiplexer.Connect(_redisContainer.ConnectionString);
            var redisDatabaseCache = multiplexer.GetDatabase();

            _databaseCacheRepository = new RedisDatabaseCacheRepository(redisDatabaseCache);

            _unleashPostgresContainer = new TestUnleashPostgresContainer(_network);
            await _unleashPostgresContainer.BuildContainer();

            _unleashWebContainer = new TestUnleashWebContainer(
                _unleashPostgresContainer.Container.Name.TrimStart('/'), 
                _network
            );
            var cancellationTokenSource = new CancellationTokenSource(20000);
            await _unleashWebContainer.BuildContainer(cancellationTokenSource.Token);

            var settings = new UnleashSettings()
            {
                AppName = UnleashAppName,
                UnleashApi = _unleashWebContainer.Endpoint,
                CustomHttpHeaders = new Dictionary<string, string>()
            {
                { "Authorization", UnleashApiKey },
            }
            };
            var unleashFactory = new UnleashClientFactory();
            // CreateClientAsync initiates the connection to the unleash server
            // Sufficient to check if unleash server is available
            IUnleash unleash = await unleashFactory.CreateClientAsync(settings, synchronousInitialization: true);

            var featureToggleRepository = new UnleashFeatureToggleRepository(unleash);
            _featureToggleService = new FeatureToggleService(featureToggleRepository, _databaseCacheRepository, _logger);
        }

        [OneTimeTearDown]
        public async Task Teardown()
        {
            await _unleashPostgresContainer.Container.StopAsync();
            await _unleashWebContainer.Container.StopAsync();
            await _network.DisposeAsync();
        }

        [Test]
        [TestCase(FeatureName.UserCreate, true)]
        [TestCase("TestShouldAlwaysBeFalse", false)]
        public async Task FeatureToggleServiceTest(string featureName, bool expectedResult)
        {
            var filter = new FeatureToggleFilter(_featureToggleService, featureName);
            var actual = await _featureToggleService.IsFeatureEnabled($"dreamer_{featureName}");

            Assert.That(actual, Is.EqualTo(expectedResult));
        }
    }
}
