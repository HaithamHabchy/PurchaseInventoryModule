using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    // This class is used for creating a new purchase order item.
    public class PurchaseOrderItemCreateDTO
    {
        [Required]
        public int? ItemId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Received Quantity must be between 1 than and 1000.")]
        public int? Quantity { get; set; }
        [Required]
        [Range(0.01, 1000, ErrorMessage = "Unit Price must be between 1 and 1000.")]
        public decimal? UnitPrice { get; set; }
    }
}
