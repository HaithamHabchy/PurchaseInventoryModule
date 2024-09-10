using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    // Represents a PurchaseOrderReceipt entity with validation rules for database storage.
    public class PurchaseReceipt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseOrderId { get; set; }
        [Required]
        public DateTime ReceiptDate { get; set; }
        [ForeignKey(nameof(PurchaseOrderId))]
        public PurchaseOrder PurchaseOrder { get; set; }
        public ICollection<PurchaseReceiptItem>? ReceiptItems { get; set; } = new List<PurchaseReceiptItem>();
    }
}
