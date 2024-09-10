namespace Common.Shared.DTOs
{
    // DTO representing a supplier's data for read operations.
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
