namespace Common.Shared.DTOs
{
    // This class represents the response returned after successfully creating a purchase order.
    public class PurchaseOrderCreationResponeDTO
    {
        public int PurchaseOrderId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
