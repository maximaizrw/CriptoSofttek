using CriptoSofttek.DataAccess.Repositories.Interfaces;
using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Movement>> GetMovementsByUserId(int userId)
        {
            return await _context.Movements.Where(m => m.UserId == userId).ToListAsync();
        }
        

    }
}
