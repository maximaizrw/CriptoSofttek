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

        public async Task<bool> AccountExists(string accountNumber)
        {
            return await _context.FiatAccounts.AnyAsync(x => x.AccountNumber == accountNumber);
        }
    }
}
