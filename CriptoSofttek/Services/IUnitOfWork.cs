using CriptoSofttek.DataAccess.Repositories;

namespace CriptoSofttek.Services
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }

        Task<int> Complete();
    }
}
