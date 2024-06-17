using Dreamer.DataAccess.Constants;
using Dreamer.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Dreamer.DataAccess
{
    public class DreamerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Jwt> Jwts { get; set; }
        public DreamerDbContext(DbContextOptions<DreamerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName(IndexNames.UserEmailUnique);

            modelBuilder.Entity<Jwt>()
                .HasOne(jwt => jwt.User)
                .WithMany(user => user.Jwts)
                .HasForeignKey(jwt => jwt.UserId);
        }
    }
}
