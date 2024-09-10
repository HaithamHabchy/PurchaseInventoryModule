using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing PurchaseOrder entities.
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public PurchaseOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

