using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing PurchaseReceiptItem entities.
    public class PurchaseReceiptItemRepository : Repository<PurchaseReceiptItem>, IPurchaseReceiptItemRepository
    {
        private readonly ApplicationDbContext _db;
        public PurchaseReceiptItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
