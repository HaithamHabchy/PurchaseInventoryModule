namespace Common.Shared.DTOs
{
    public class PurchaseReceiptDTO
    {
        public ICollection<PurchaseReceiptItemDTO> ReceiptItems { get; set; } = new List<PurchaseReceiptItemDTO>();
    }
}
