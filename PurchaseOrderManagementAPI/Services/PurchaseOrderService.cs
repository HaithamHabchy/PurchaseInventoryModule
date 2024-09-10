using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using Common.Shared.SharedClasses;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using PurchaseOrderManagementAPI.Services.IServices;

namespace PurchaseOrderManagementAPI.Services
{
    // Service class responsible for handling operations related to purchase orders.
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Main method to create a purchase order based on the provided DTO.
        public async Task<PurchaseOrderCreationResponeDTO> CreatePurchaseOrderAsync(PurchaseOrderCreateDTO createDTO)
        {
            // Validate the provided purchase order details.
            await ValidatePurchaseOrder(createDTO);

            // Validate the order items and calculate the total amount.
            var result = await ValidateOrderItems(createDTO);
            decimal totalAmount = result.totalAmount;
            List<PurchaseOrderItem> purchaseOrderItems = result.purchaseOrderItems;

            // Create the purchase order and return the response DTO.
            PurchaseOrder purchaseOrder = await CreatePurchaseOrder(createDTO, totalAmount, purchaseOrderItems);

            // Return the response DTO with the created order ID and total order amount.
            return new PurchaseOrderCreationResponeDTO
            {
                PurchaseOrderId = purchaseOrder.Id,
                TotalAmount = purchaseOrder.TotalAmount
            };
        }

        // Method to create the purchase order and save it to the database.
        private async Task<PurchaseOrder> CreatePurchaseOrder(PurchaseOrderCreateDTO createDTO, decimal totalAmount, List<PurchaseOrderItem> purchaseOrderItems)
        {
            var purchaseOrder = new PurchaseOrder
            {
                SupplierId = createDTO.SupplierId.Value,
                OrderDate = DateTime.Now,
                UpdatedDate = null,
                Status = SD.PO_Status_Pending,
                TotalAmount = totalAmount
            };

            // Add the purchase order to the database.
            await _unitOfWork.PurchaseOrder.CreateAsync(purchaseOrder);
            await _unitOfWork.SaveAsync();

            int purchaseOrderId = purchaseOrder.Id;

            // Assign the purchase order ID to each order item.
            foreach (var orderItem in purchaseOrderItems)
            {
                orderItem.PurchaseOrderId = purchaseOrderId;
            }

            // Add the order items to the database.
            await _unitOfWork.PurchaseOrderItem.AddRangeAsync(purchaseOrderItems);

            // Save the changes to the database.
            await _unitOfWork.SaveAsync();
            return purchaseOrder;
        }

        // Method to validate the order items and calculate the total amount.
        private async Task<dynamic> ValidateOrderItems(PurchaseOrderCreateDTO createDTO)
        {
            decimal totalAmount = 0;
            var purchaseOrderItems = new List<PurchaseOrderItem>();

            foreach (var orderItem in createDTO.OrderItems)
            {
                // Retrieve the item details from the database.
                Item item = await _unitOfWork.Item.GetAsync(i => i.Id == orderItem.ItemId);
                if (item == null)
                {
                    throw new ApiNotFoundException(new List<string> { $"Item with ID {orderItem.ItemId} not found." });
                }

                // Calculate the total amount
                totalAmount += orderItem.Quantity.Value * orderItem.UnitPrice.Value;

                // Add the order item to the list.
                purchaseOrderItems.Add(new PurchaseOrderItem
                {
                    ItemId = orderItem.ItemId.Value,
                    UnitPrice = orderItem.UnitPrice.Value,
                    Quantity = orderItem.Quantity.Value
                });
            }

            // Return the total amount and list of order items.
            return new { totalAmount = totalAmount, purchaseOrderItems = purchaseOrderItems };
        }

        // Method to validate the purchase order details.
        private async Task ValidatePurchaseOrder(PurchaseOrderCreateDTO createDTO)
        {
            // Check if the order items list is not empty.
            if (createDTO.OrderItems == null || !createDTO.OrderItems.Any())
            {
                throw new ApiBadRequestException(new List<string> { "The orderItems array must contain at least one item." });
            }

            // Check if the supplier ID is valid.
            if (createDTO.SupplierId <= 0)
            {
                throw new ApiBadRequestException(new List<string> { "Invalid supplier ID." });
            }

            // Retrieve and validate the supplier.
            Supplier supplier = await _unitOfWork.Supplier.GetAsync(u => u.Id == createDTO.SupplierId);
            if (supplier == null)
            {
                throw new ApiNotFoundException(new List<string> { "Supplier ID not found." });
            }

            // Check for duplicate items in the order.
            var duplicateItems = createDTO.OrderItems
                .GroupBy(i => i.ItemId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateItems.Any())
            {
                throw new ApiBadRequestException(new List<string> { $"Duplicate items found: {string.Join(", ", duplicateItems)}." });
            }
        }

        // Method to update the status of a purchase order.
        public async Task UpdatePurchaseOrderStatusAsync(PurchaseOrderStatusUpdateDTO updateDTO)
        {
            // Retrieve and validate the purchase order.
            PurchaseOrder po = await RetrieveAndValidatePOStatus(updateDTO);

            // Update the purchase order status.
            po.UpdatedDate = DateTime.UtcNow;
            po.Status = updateDTO.Status.ToLower();

            // Save the changes to the database.
            await _unitOfWork.PurchaseOrder.UpdateAsync(po);
            await _unitOfWork.SaveAsync();
        }

        // Method to retrieve and validate the purchase order status.
        private async Task<PurchaseOrder> RetrieveAndValidatePOStatus(PurchaseOrderStatusUpdateDTO updateDTO)
        {
            // Validate the provided status.
            if (updateDTO.Status.ToLower() != SD.PO_Status_Completed && updateDTO.Status.ToLower() != SD.PO_Status_Cancelled)
            {
                throw new ApiBadRequestException(new List<string> { $"Invalid status {updateDTO.Status}" });
            }

            // Retrieve and validate the purchase order.
            PurchaseOrder po = await _unitOfWork.PurchaseOrder.GetAsync(i => i.Id == updateDTO.PurchaseOrderId, "OrderItems");
            if (po == null)
            {
                throw new ApiNotFoundException(new List<string> { $"Purchase Order with ID {updateDTO.PurchaseOrderId} not found." });
            }

            // Check if the purchase order is in a valid state for status update.
            if (po.Status != SD.PO_Status_Pending)
            {
                throw new ApiNotFoundException(new List<string> { $"Purchase Order with ID {updateDTO.PurchaseOrderId} is already {po.Status}. Only pending orders can be updated." });
            }

            return po;
        }
    }
}
