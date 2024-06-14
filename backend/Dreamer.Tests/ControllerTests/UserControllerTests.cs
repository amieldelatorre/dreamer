using Dreamer.Api.Controllers;
using Dreamer.Cache;
using Dreamer.DataAccess;
using Dreamer.DataAccess.Repository;
using Dreamer.Domain.DTOs;
using Dreamer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using Tests.Setup;

namespace Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly TestDreamerPostgresContainer _postgresContainer;
        private readonly TestDreamerRedisContainer _redisContainer;
        private readonly ILogger _logger;
        private readonly ISqlErrorUnpacker _errorUnpacker;
        private IUserRepository _userRepository;
        private IDatabaseCacheRepository _databaseCacheRepository;
        private IUserCache _userCache;
        private IUserService _userService;
        private UserController _userController;

        public UserControllerTests()
        {
            _logger = SerilogLogger.GetLogger();
            _postgresContainer = new TestDreamerPostgresContainer();
            _redisContainer = new TestDreamerRedisContainer();
            _errorUnpacker = new PostgresErrorUnpacker();
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            await _postgresContainer.BuildContainer();
            await _redisContainer.BuildContainer();
            var builder = new DbContextOptionsBuilder<DreamerDbContext>().UseNpgsql(_postgresContainer.ConnectionString);
            var options = builder.Options;
            var dbContext = new DreamerDbContext(options);

            var multiplexer = ConnectionMultiplexer.Connect(_redisContainer.ConnectionString);
            var redisDatabaseCache = multiplexer.GetDatabase();

            _databaseCacheRepository = new RedisDatabaseCacheRepository(redisDatabaseCache);
            _userRepository = new PgsqlUserRepository(dbContext);
            _userCache = new UserCache(_userRepository, _databaseCacheRepository, _logger);

            _userService = new UserService(_userCache, _errorUnpacker, _logger);
            _userController = new UserController(_userService, _logger);
        }

        [OneTimeTearDown]
        public async Task Teardown()
        {
            await _postgresContainer.Container.StopAsync();
            await _redisContainer.Container.StopAsync();
        }

        [Test]
        [TestCase("01")]
        [TestCase("02")]
        [TestCase("03")]
        [TestCase("04")]
        [TestCase("05")]
        public async Task PostUserTest(string filePrefix)
        {
            var jsonString = await File.ReadAllTextAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "Cases", "UserController", "PostUser", $"{filePrefix}.json"));
            var testCase = JsonConvert.DeserializeObject<PostUserTestInput>(jsonString);
            Assert.That(testCase, Is.Not.Null);

            var controllerResult = (await _userController.PostUser(testCase.UserCreateObj)).Result as ObjectResult;
            Assert.That(controllerResult, Is.Not.Null);
            var controllerResultValue = controllerResult.Value;
            Assert.That(controllerResultValue, Is.Not.Null);

            Result<UserView> userResult = (Result<UserView>)controllerResultValue;

            Assert.Multiple(() =>
            {
                Assert.That(userResult.Errors, Has.Count.EqualTo(testCase.ExpectedResult.Errors.Count));
                Assert.That(userResult.RequestResultStatus, Is.EqualTo(testCase.ExpectedResult.RequestResultStatus));
            });

            if (userResult.Errors.Count == 0)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(userResult, Is.Not.Null);
                    Assert.That(userResult.Item, Is.Not.Null);
                    Assert.That(testCase.ExpectedResult.Item, Is.Not.Null);
                });
                Assert.Multiple(() =>
                {
                    Assert.That(userResult, Is.Not.Null);
                    Assert.That(userResult.Item.FirstName, Is.EqualTo(testCase.ExpectedResult.Item.FirstName));
                    Assert.That(userResult.Item.LastName, Is.EqualTo(testCase.ExpectedResult.Item.LastName));
                    Assert.That(userResult.Item.Email, Is.EqualTo(testCase.ExpectedResult.Item.Email));
                });
            }

            Assert.Multiple(() =>
            {
                foreach (var error in testCase.ExpectedResult.Errors)
                {
                    Assert.That(userResult.Errors[error.Key], Is.EqualTo(error.Value));
                }
            });
        }

        private class PostUserTestInput
        {
            public required UserCreate UserCreateObj { get; set; }
            public required Result<UserView> ExpectedResult { get; set; }
        }
    }
}
