using Dreamer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
