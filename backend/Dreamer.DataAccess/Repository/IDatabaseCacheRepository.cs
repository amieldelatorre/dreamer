using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.DataAccess.Repository
{
    public interface IDatabaseCacheRepository
    {
        Task SetKey<T>(string key, T value, int expireSeconds);
        Task<T?> GetKey<T>(string key);
        Task RemoveKey(string key);
    }
}
