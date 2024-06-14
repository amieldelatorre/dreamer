using Dreamer.Api.Utils;
using Dreamer.Cache;
using Dreamer.DataAccess;
using Dreamer.DataAccess.Repository;
using Dreamer.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);


// Logger for start up
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch(Dreamer.Api.Utils.LogLevel.Get()))
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Unleash", LogEventLevel.Warning)
    .CreateLogger();


// Add logging
builder.Host.UseSerilog((_, configuration) =>
    configuration
        .WriteTo.Console(new JsonFormatter())
        .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch(Dreamer.Api.Utils.LogLevel.Get()))
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.WithThreadId()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
);


// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register db repositories
Log.Logger.Information("Registering database repositories");
var connectionString = PostgresClient.GetConnectionString();
var databaseStartup = new DatabaseStartup(connectionString);
if (!databaseStartup.Configure())
    Environment.Exit(1);

builder.Services.AddDbContext<DreamerDbContext>(
    options => options.UseNpgsql(connectionString)
);
builder.Services.AddScoped<IUserRepository, PgsqlUserRepository>();
builder.Services.AddScoped<ISqlErrorUnpacker, PostgresErrorUnpacker>();


// Register database cache repository
Log.Logger.Information("Registering database cache repository");
var redisDatabaseCache = RedisClient.GetClient();
builder.Services.AddSingleton(redisDatabaseCache);
builder.Services.AddScoped<IDatabaseCacheRepository, RedisDatabaseCacheRepository>();
builder.Services.AddScoped<IUserCache, UserCache>();


// Register feature toggle service
Log.Logger.Information("Registering feature toggle service");
var unleash = await UnleashClient.GetClient();
builder.Services.AddSingleton(unleash);
builder.Services.AddScoped<IFeatureToggleRepository, UnleashFeatureToggleRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFeatureToggleService, FeatureToggleService>();

// Configure CORS
// TODO: Properly set cors allowed origins
var allowDevOrigin = "allowDevOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowDevOrigin,
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:8079")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowDevOrigin);

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

Log.Logger.Information("Dreamer.Api is ready");
app.Run();
