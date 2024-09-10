using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing Supplier entities.
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly ApplicationDbContext _db;
        public SupplierRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
