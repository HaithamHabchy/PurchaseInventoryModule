using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class PurchaseReceiptItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseReceiptId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ReceivedQuantity { get; set; }
        [ForeignKey(nameof(PurchaseReceiptId))]
        public PurchaseReceipt PurchaseReceipt { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }
    }
}
