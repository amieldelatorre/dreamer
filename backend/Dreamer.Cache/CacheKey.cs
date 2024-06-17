using Dreamer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.Cache
{
    public static class CacheKey
    {
        private const string FeatureToggleCachePrefix = "FeatureToggle";
        public static string FeatureToggleCacheKey(string featureName) => $"{FeatureToggleCachePrefix}:{featureName}";


        private const string UserCachePrefix = "User";
        public static string UserIdCacheKey(Guid userId) => $"{UserCachePrefix}:Id:{userId}";
        public static string UserEmailCacheKey(string email) => $"{UserCachePrefix}:Email:{email}";


        public const string JwtCachePrefix = "Jwt";
        public static string JwtIdCacheKey(Guid jwtId) => $"{JwtCachePrefix}:Id:{jwtId}";
    }
}
