using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    // DTO used to create a new supplier.
    public class SupplierCreateDTO
    {
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
