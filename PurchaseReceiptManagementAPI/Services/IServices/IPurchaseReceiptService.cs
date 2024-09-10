using Common.Shared.DTOs;

namespace PurchaseReceiptManagementAPI.Services.IServices
{
    public interface IPurchaseReceiptService
    {
        Task<PurchaseReceiptCreationResponeDTO> CreatePurchaseReceiptAsync(PurchaseReceiptCreateDTO createDTO);
        Task<PurchaseReceiptDTO> GetPurchaseReceiptByIdAsync(int id);
    }
}