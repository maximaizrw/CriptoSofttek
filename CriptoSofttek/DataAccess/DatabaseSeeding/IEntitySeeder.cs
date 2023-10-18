using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.DatabaseSeeding
{
    public interface IEntitySeeder
    {
        void SeedDatabase(ModelBuilder modelBuilder);
    }
}
