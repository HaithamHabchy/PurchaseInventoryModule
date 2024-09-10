using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    // Represents a PurchaseOrder entity with validation rules for database storage.
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }

        public ICollection<PurchaseOrderItem>? OrderItems { get; set; } = new List<PurchaseOrderItem>();

    }
}
