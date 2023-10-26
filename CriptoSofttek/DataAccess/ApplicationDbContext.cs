using CriptoSofttek.DataAccess.DatabaseSeeding;
using CriptoSofttek.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FiatAccount> FiatAccounts { get; set; }
        public DbSet<CryptoAccount> CryptoAccounts { get; set; }
        public DbSet<Movement> Movements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var seeders = new List<IEntitySeeder>
            {
                new UserSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.SeedDatabase(modelBuilder);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
