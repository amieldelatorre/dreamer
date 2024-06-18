using Dreamer.Api.Controllers;
using Dreamer.Cache;
using Dreamer.DataAccess.Repository;
using Dreamer.Domain.Services;
using Dreamer.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using Tests.Setup;
using Dreamer.DataAccess;
using Dreamer.Domain.DTOs;

namespace Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly TestDreamerPostgresContainer _postgresContainer;
    private readonly TestDreamerRedisContainer _redisContainer;
    private readonly ILogger _logger;
    private IUserRepository _userRepository;
    private IJwtRepository _jwtRepository;
    private IDatabaseCacheRepository _databaseCacheRepository;
    private IJwtService _jwtService;
    private IJwtCache _jwtCache;
    private IUserCache _userCache;
    private AuthController _authController;

    public AuthControllerTests()
    {
        _logger = SerilogLogger.GetLogger();
        _postgresContainer = new TestDreamerPostgresContainer();
        _redisContainer = new TestDreamerRedisContainer();
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
        _jwtRepository = new PgsqlJwtRepository(dbContext);
        _userCache = new UserCache(_userRepository, _databaseCacheRepository, _logger);
        _jwtCache = new JwtCache(_jwtRepository, _databaseCacheRepository, _logger);

        var userLoginCredentialsValidator = new UserLoginCredentialsDtoValidator();

        _jwtService = new JwtService(_jwtCache, _userCache, _logger);

        _authController = new AuthController(userLoginCredentialsValidator, _jwtService, _logger);
    }

    [OneTimeTearDown]
    public async Task Teardown()
    {
        await _postgresContainer.Container.StopAsync();
        await _redisContainer.Container.StopAsync();
        _authController.Dispose();
    }

    [Test]
    [TestCase("01")]
    [TestCase("02")]
    [TestCase("03")]
    [TestCase("04")]
    [TestCase("05")]
    [TestCase("06")]
    public async Task LoginTest(string filePrefix)
    {
        var jsonString = await File.ReadAllTextAsync(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "Cases", "AuthController", "Login", $"{filePrefix}.json"));
        var testCase = JsonConvert.DeserializeObject<LoginTestInput>(jsonString);
        Assert.That(testCase, Is.Not.Null);

        var controllerResult = (await _authController.Login(testCase.UserLoginCredentials)).Result as ObjectResult;
        Assert.That(controllerResult, Is.Not.Null);
        var controllerResultValue = controllerResult.Value;
        Assert.That(controllerResultValue, Is.Not.Null);

        Result<JwtCreateView> loginResult = (Result<JwtCreateView>)controllerResultValue;

        Assert.Multiple(() =>
        {
            Assert.That(loginResult.Errors, Has.Count.EqualTo(testCase.ExpectedResult.Errors.Count));
            Assert.That(loginResult.RequestResultStatus, Is.EqualTo(testCase.ExpectedResult.RequestResultStatus));
        });

        if (loginResult.Errors.Count == 0)
        {
            Assert.Multiple(() =>
            {
                Assert.That(loginResult, Is.Not.Null);
                Assert.That(loginResult.Item, Is.Not.Null);
                Assert.That(testCase.ExpectedResult.Item, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(loginResult, Is.Not.Null);
                Assert.That(loginResult.Item.IsDisabled, Is.EqualTo(testCase.ExpectedResult.Item.IsDisabled));
                Assert.That(loginResult.Item.UserId, Is.EqualTo(testCase.ExpectedResult.Item.UserId));
            });
        }

        Assert.Multiple(() =>
        {
            foreach (var error in testCase.ExpectedResult.Errors)
            {
                Assert.That(loginResult.Errors[error.Key], Is.EqualTo(error.Value));
            }
        });
    }

    private class LoginTestInput
    {
        public required UserLoginCredentialsDto UserLoginCredentials { get; set; }
        public required Result<JwtCreateView> ExpectedResult { get; set; }
    }
}
