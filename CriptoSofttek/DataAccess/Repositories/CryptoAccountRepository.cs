using CriptoSofttek.Entities;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class CryptoAccountRepository : Repository<CryptoAccount>
    {
        public CryptoAccountRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
