using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unleash;

namespace Dreamer.DataAccess.Repository
{
    public class UnleashFeatureToggleRepository(IUnleash unleashClient) : IFeatureToggleRepository
    {
        public bool FeatureIsEnabled(string featureName)
        {
            return unleashClient.IsEnabled(featureName);
        }
    }
}
