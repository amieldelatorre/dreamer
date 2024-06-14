using Dreamer.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Dreamer.DataAccess.Repository
{
    public class PgsqlUserRepository(DreamerDbContext dbContext) : IUserRepository
    {
        public async Task Create(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
