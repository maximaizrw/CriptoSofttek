using CriptoSofttek.DataAccess.Repositories.Interfaces;
using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
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

        public async Task SaveTransaction(int userId, decimal amount, string currency, string type)
        {
            var movementDTO = new MovementDTO
            {
                UserId = userId,
                Currency = currency,
                Amount = amount,
                TypeMovement = type
            };

            var movement = new Movement(movementDTO);
            await _context.Movements.AddAsync(movement);
        }


    }
}
