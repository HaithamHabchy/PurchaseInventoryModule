using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    // DTO used to update purchase order status.
    public class PurchaseOrderStatusUpdateDTO
    {
        [Required]
        public int? PurchaseOrderId { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
