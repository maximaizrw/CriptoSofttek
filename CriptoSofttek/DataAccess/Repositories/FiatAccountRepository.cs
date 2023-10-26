using CriptoSofttek.DataAccess.Repositories.Interfaces;
using CriptoSofttek.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class FiatAccountRepository : Repository<FiatAccount>, IFiatAccountRepository
    {
        public FiatAccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AccountExists(int accountNumber)
        {
            return await _context.FiatAccounts.AnyAsync(x => x.AccountNumber == accountNumber);
        }

        public async Task<FiatAccount> GetFiatAccountByUserId(int userId)
        {
            return await _context.FiatAccounts.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<FiatAccount>> GetFiatAccountsByUserId(int userId)
        {
            return await _context.FiatAccounts.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<FiatAccount> GetFiatAccountByAccountNumber(int accountNumber)
        {
            return await _context.FiatAccounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        }

        public async Task<FiatAccount> GetFiatAccountByCBU(string cbu)
        {
            return await _context.FiatAccounts.FirstOrDefaultAsync(x => x.CBU.Equals(cbu));
        }



    }
}
