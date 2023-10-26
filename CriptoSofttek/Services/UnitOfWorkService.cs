using CriptoSofttek.DataAccess;
using CriptoSofttek.DataAccess.Repositories;

namespace CriptoSofttek.Services
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UserRepository UserRepository { get; private set; }
        public FiatAccountRepository FiatAccountRepository { get; private set; }
        public CryptoAccountRepository CryptoAccountRepository { get; private set; }
        public MovementRepository MovementRepository { get; private set; }

        public UnitOfWorkService(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
            FiatAccountRepository = new FiatAccountRepository(_context);
            CryptoAccountRepository = new CryptoAccountRepository(_context);
            MovementRepository = new MovementRepository(_context);
        }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
