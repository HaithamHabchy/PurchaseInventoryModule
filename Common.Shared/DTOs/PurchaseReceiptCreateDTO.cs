using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    public class PurchaseReceiptCreateDTO
    {
        [Required]
        public int? PurchaseOrderId { get; set; }
        [Required]
        public ICollection<PurchaseReceiptItemCreateDTO> ReceiptItems { get; set; } = new List<PurchaseReceiptItemCreateDTO>();
    }
}
