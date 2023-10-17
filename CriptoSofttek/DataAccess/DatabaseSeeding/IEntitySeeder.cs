using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.DatabaseSeeding
{
    public interface IEntitySeeder
    {
        void SeedDatabse(ModelBuilder modelBuilder);
    }
}
