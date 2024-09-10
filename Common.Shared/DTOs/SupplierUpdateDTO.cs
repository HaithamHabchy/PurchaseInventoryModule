using System.ComponentModel.DataAnnotations;

namespace Common.Shared.DTOs
{
    // DTO used to update a supplier.
    public class SupplierUpdateDTO
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}
