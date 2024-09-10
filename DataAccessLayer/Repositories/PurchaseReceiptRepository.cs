using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing PurchaseReceipt entities.
    public class PurchaseReceiptRepository : Repository<PurchaseReceipt>, IPurchaseReceiptRepository
    {
        private readonly ApplicationDbContext _db;
        public PurchaseReceiptRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
