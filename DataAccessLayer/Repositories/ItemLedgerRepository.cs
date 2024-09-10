using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Repository class for managing ItemLedger entities.
    public class ItemLedgerRepository : Repository<ItemLedger>, IItemLedgerRepository
    {
        private readonly ApplicationDbContext _db;
        public ItemLedgerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
