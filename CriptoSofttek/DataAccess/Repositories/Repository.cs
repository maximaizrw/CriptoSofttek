using CriptoSofttek.DataAccess.Repositories.Interfaces;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
