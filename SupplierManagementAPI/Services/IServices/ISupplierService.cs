using Common.Shared.DTOs;

namespace SupplierManagementAPI.Services.IServices
{
    public interface ISupplierService
    {
        Task<SupplierDTO> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> CreateSupplierAsync(SupplierCreateDTO createDTO);
        Task DeleteSupplierAsync(int id);
        Task UpdateSupplierAsync(int id, SupplierUpdateDTO updateDTO);

    }
}