using CriptoSofttek.DataAccess.Repositories.Interfaces;
using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        //Crear update y modificar el id de las cuentas
        
        
        
        public async Task<int> GetIdByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();
        }
        
        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
        public async Task<User?> AuthenticateCredentials(AuthenticateDTO dto)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == PasswordEncryptHelper.EncryptPassword(dto.Password, dto.Email));
        }
    }
}
