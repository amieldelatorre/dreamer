using Dreamer.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Dreamer.DataAccess.Repository;

public class PgsqlJwtRepository(DreamerDbContext dbContext) : IJwtRepository
{
    public async Task Create(Jwt jwt)
    {
        await dbContext.Jwts.AddAsync(jwt);
        await dbContext.SaveChangesAsync();
    }
}
