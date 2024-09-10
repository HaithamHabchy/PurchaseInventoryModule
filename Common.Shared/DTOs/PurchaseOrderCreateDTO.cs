using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    // This class represents the data required to create a new purchase order.
    public class PurchaseOrderCreateDTO
    {
        [Required]
        public int? SupplierId { get; set; }
        [Required]
        public ICollection<PurchaseOrderItemCreateDTO> OrderItems { get; set; } = new List<PurchaseOrderItemCreateDTO>();
    }
}
