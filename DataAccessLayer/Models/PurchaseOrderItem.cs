using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    // Represents a PurchaseOrderItem entity with validation rules for database storage.
    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseOrderId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [ForeignKey(nameof(PurchaseOrderId))]
        public PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }
    }
}
