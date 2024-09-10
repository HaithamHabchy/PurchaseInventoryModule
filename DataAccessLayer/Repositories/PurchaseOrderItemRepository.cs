using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing PurchaseOrderItem entities.
    public class PurchaseOrderItemRepository : Repository<PurchaseOrderItem>, IPurchaseOrderItemRepository
    {
        private readonly ApplicationDbContext _db;
        public PurchaseOrderItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
