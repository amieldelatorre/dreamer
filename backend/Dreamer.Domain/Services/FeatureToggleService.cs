using Dreamer.Cache;
using Dreamer.DataAccess.Repository;
using Serilog;

namespace Dreamer.Domain.Services
{
    public class FeatureToggleService : IFeatureToggleService
    {
        private readonly IFeatureToggleRepository _featureToggleRepository;
        private readonly IDatabaseCacheRepository _databaseCacheRepository;
        private readonly ILogger _logger;
        private const int DefaultCacheSeconds = 30;

        public FeatureToggleService(IFeatureToggleRepository featureToggleRepository, IDatabaseCacheRepository databaseCacheRepository, ILogger logger)
        {
            _featureToggleRepository = featureToggleRepository;
            _databaseCacheRepository = databaseCacheRepository;
            _logger = logger;
        }

        public async Task<bool> IsFeatureEnabled(string featureName)
        {
            var cacheKey = CacheKey.FeatureToggleCacheKey(featureName);
            var featureEnabled = false;

            try
            {   
                // Use a string here because the default boolean value is false
                string? cacheValue = await _databaseCacheRepository.GetKey<string>(cacheKey);
                if (cacheValue != null)
                {
                    _ = bool.TryParse(cacheValue, out featureEnabled);
                    _logger.Debug("Got {cacheKey} from cache", cacheKey);
                    return featureEnabled;
                }

                featureEnabled = _featureToggleRepository.FeatureIsEnabled(featureName);

                try 
                { 
                    await _databaseCacheRepository.SetKey(cacheKey, featureEnabled.ToString(), DefaultCacheSeconds);
                } catch
                {
                    _logger.Error("Could not set cache value for feature flag '{cacheKey}'", cacheKey);
                }

                _logger.Debug("Got {cacheKey} from flag provider", cacheKey);
                return featureEnabled;
            }
            catch
            {
                return featureEnabled;
            } 
        }
    }
}
