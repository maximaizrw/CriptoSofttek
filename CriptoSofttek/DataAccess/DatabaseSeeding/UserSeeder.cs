using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.DatabaseSeeding
{
    public class UserSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Maxi",
                    LastName = "Maiz",
                    Email = "maxi@gmail.com",
                    Password = PasswordEncryptHelper.EncryptPassword("1234", "maxi@gmail.com")
                });
        }
    }
}
