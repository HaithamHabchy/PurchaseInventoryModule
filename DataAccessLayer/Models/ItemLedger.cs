using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class ItemLedger
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string TransactionType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }
    }
}
