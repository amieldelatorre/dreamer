using Dreamer.DataAccess.Models;
using Dreamer.DataAccess.Repository;
using Dreamer.Shared.Constants;
using Serilog;

namespace Dreamer.Cache;

public class UserCache(
    IUserRepository userRepository,
    IDatabaseCacheRepository databaseCacheRepository,
    ILogger logger)
    : IUserCache
{
    private const int DefaultCacheSeconds = 60;

    public async Task Create(User user)
    {
        await userRepository.Create(user);

        // cacheKey = "User:Id:009844ac-2260-419e-917c-a30da49dcab7"
        var cacheKey = CacheKey.UserIdCacheKey(user.Id);
        try
        {
            await databaseCacheRepository.SetKey<User>(cacheKey, user, DefaultCacheSeconds);
        } catch (Exception ex)
        {
            logger.Error("{featureName}: could not add user {userId} to cache. Exception: {error}",
                FeatureName.UserCreate, user.Id, ex.Message);
        }
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        User? user;
        var cacheKey = CacheKey.UserEmailCacheKey(email);

        // cacheKey = "User:Email:email@example.invalid"
        try
        {
            user = await databaseCacheRepository.GetKey<User>(cacheKey);
            if (user != null)
                return user;
        }
        catch (Exception ex)
        {
            logger.Error("{featureName}: could not get user from cache. Exception: {error}",
                FeatureName.UserGetByEmail, ex.Message);
        }

        user = await userRepository.GetUserByEmail(email);
        if (user != null)
            await databaseCacheRepository.SetKey(cacheKey, user, DefaultCacheSeconds);
        return user;
    }
}