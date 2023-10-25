using CriptoSofttek.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class CryptoAccountRepository : Repository<CryptoAccount>
    {
        public CryptoAccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CryptoAccount> GetCryptoAccountByUserId(int id)
        {
            return await _context.CryptoAccounts.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<IEnumerable<CryptoAccount>> GetCryptoAccountsByUserId(int userId)
        {
            return await _context.CryptoAccounts.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<CryptoAccount> GetCryptoAccountByUUID(string uuid)
        {
            return await _context.CryptoAccounts.FirstOrDefaultAsync(x => x.UUID.Equals(uuid));
        }
    }
}
