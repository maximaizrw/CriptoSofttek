using CriptoSofttek.DataAccess.Repositories;

namespace CriptoSofttek.Services
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public FiatAccountRepository FiatAccountRepository { get; }
        public CryptoAccountRepository CryptoAccountRepository { get; }
        public MovementRepository MovementRepository { get; }

        Task<int> Complete();
    }
}
