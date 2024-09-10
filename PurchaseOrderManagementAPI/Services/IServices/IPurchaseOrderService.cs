using Common.Shared.DTOs;

namespace PurchaseOrderManagementAPI.Services.IServices
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderCreationResponeDTO> CreatePurchaseOrderAsync(PurchaseOrderCreateDTO createDTO);
        Task UpdatePurchaseOrderStatusAsync(PurchaseOrderStatusUpdateDTO updateDTO);
    }
}
