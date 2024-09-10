namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IItemRepository Item { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        ISupplierRepository Supplier { get; }
        IPurchaseOrderItemRepository PurchaseOrderItem { get; }
        IPurchaseReceiptRepository PurchaseReceipt { get; }
        IPurchaseReceiptItemRepository PurchaseReceiptItem { get; }
        IItemLedgerRepository ItemLedger { get; }
        Task SaveAsync();
    }
}