using Dreamer.DataAccess.Models;
using Dreamer.DataAccess.Repository;
using Dreamer.Shared.Constants;

namespace Dreamer.Cache;

public class JwtCache(
    IJwtRepository jwtRepository, 
    IDatabaseCacheRepository databaseCacheRepository,
    Serilog.ILogger logger) : IJwtCache
{
    private const int DefaultCacheSeconds = 60;

    public async Task Create(Jwt jwt)
    {
        await jwtRepository.Create(jwt);

        // cacheKey = "Jwt:Id:009844ac-2260-419e-917c-a30da49dcab7"
        var cacheKey = CacheKey.JwtIdCacheKey(jwt.Id);
        try
        {
            await databaseCacheRepository.SetKey<Jwt>(cacheKey, jwt, DefaultCacheSeconds);
        }
        catch (Exception ex)
        {
            logger.Error("{featureName}: could not add jwt {jwtId} to cache. Exception: {error}",
                FeatureName.JwtCreate, jwt.Id, ex.Message);
        }
    }
}
