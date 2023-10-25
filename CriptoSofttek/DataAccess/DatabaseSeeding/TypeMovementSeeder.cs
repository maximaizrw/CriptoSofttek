using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.DatabaseSeeding
{
    public class TypeMovementSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeMovement>().HasData(
                new TypeMovement
                {
                    Id = 1,
                    description = "Deposit"
                },
                new TypeMovement
                {
                    Id = 2,
                    description = "Withdrawal"
                },
                new TypeMovement
                {
                    Id = 3,
                    description = "Transfer"
                },
                new TypeMovement
                {
                    Id = 4,
                    description = "PurchaseCrypto"
                });


        }
    }
}
