using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    // Represents a supplier entity with validation rules for database storage.
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}