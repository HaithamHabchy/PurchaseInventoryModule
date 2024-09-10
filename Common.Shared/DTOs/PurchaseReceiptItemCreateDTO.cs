using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    public class PurchaseReceiptItemCreateDTO
    {
        [Required]
        public int? ItemId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Received Quantity must be between 1 than and 1000.")]
        public int? ReceivedQuantity { get; set; }
    }
}
