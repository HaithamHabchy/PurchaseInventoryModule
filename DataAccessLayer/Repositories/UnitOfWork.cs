using DataAccessLayer.Data;
using DataAccessLayer.Repositories.IRepositories;

namespace DataAccessLayer.Repositories
{
    // Implements the Unit of Work pattern.
    // This class manages the repositories and coordinates the saving of changes to the database.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ISupplierRepository Supplier { get; private set; }
        public IPurchaseOrderRepository PurchaseOrder { get; private set; }
        public IItemRepository Item { get; private set; }
        public IPurchaseOrderItemRepository PurchaseOrderItem { get; private set; }
        public IPurchaseReceiptRepository PurchaseReceipt { get; private set; }
        public IPurchaseReceiptItemRepository PurchaseReceiptItem { get; private set; }
        public IItemLedgerRepository ItemLedger { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Supplier = new SupplierRepository(_db);
            PurchaseOrder = new PurchaseOrderRepository(_db);
            Item = new ItemRepository(_db);
            PurchaseOrderItem = new PurchaseOrderItemRepository(_db);
            PurchaseReceipt = new PurchaseReceiptRepository(_db);
            PurchaseReceiptItem = new PurchaseReceiptItemRepository(_db);
            ItemLedger = new ItemLedgerRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
