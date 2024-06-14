using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.Domain.Services
{
    public interface IFeatureToggleService
    {
        Task<bool> IsFeatureEnabled(string featureName);
    }
}
