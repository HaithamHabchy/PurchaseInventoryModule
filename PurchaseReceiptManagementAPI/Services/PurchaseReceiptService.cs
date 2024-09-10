using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using Common.Shared.SharedClasses;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using PurchaseReceiptManagementAPI.Services.IServices;

namespace PurchaseReceiptManagementAPI.Services
{
    // Service class responsible for handling operations related to purchase receipt.
    public class PurchaseReceiptService : IPurchaseReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseReceiptService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Main method to create a purchase receipt based on the provided DTO.
        public async Task<PurchaseReceiptCreationResponeDTO> CreatePurchaseReceiptAsync(PurchaseReceiptCreateDTO createDTO)
        {
            // Validate the purchase order and get details.
            PurchaseOrder purchaseOrder = await ValidatePurchaseReceipt(createDTO);

            // Validate receipt items and prepare the list for creation.
            List<PurchaseReceiptItem> purchaseReceiptItems = await ValidateReceiptItem(createDTO, purchaseOrder);

            // Create the purchase receipt and add items to it.
            int purchaseReceiptId = await CreatePurchaseReceipt(createDTO, purchaseReceiptItems);

            // Return the response DTO with the created receipt ID.
            return new PurchaseReceiptCreationResponeDTO
            {
                PurchaseReceiptId = purchaseReceiptId,
            };
        }
        // Main method to retrieve a PurchaseReceipt by ID, including its receipt items and corresponding item names.
        public async Task<PurchaseReceiptDTO> GetPurchaseReceiptByIdAsync(int id)
        {
            // Validate the provided Purchase Receipt ID (must be greater than 0).
            ValidatePurchaseReceiptId(id);

            // Fetch the PurchaseReceipt entity along with its related ReceiptItems from the database.
            PurchaseReceipt purchaseReceipt = await GetPurchaseReceiptAsync(id);

            // Convert the PurchaseReceipt entity to a DTO, including the receipt items and item names.
            PurchaseReceiptDTO purchaseReceiptDTO = await MapToPurchaseReceiptDTO(purchaseReceipt);

            // Return the PurchaseReceiptDTO to the caller.
            return purchaseReceiptDTO;
        }

        // Method to create a new purchase receipt and associates receipt items with it.
        private async Task<int> CreatePurchaseReceipt(PurchaseReceiptCreateDTO createDTO, List<PurchaseReceiptItem> purchaseReceiptItems)
        {
            // Create a new PurchaseReceipt object.
            var purchaseReceipt = new PurchaseReceipt
            {
                PurchaseOrderId = createDTO.PurchaseOrderId.Value,
                ReceiptDate = DateTime.Now,
            };

            // Add the new receipt to the database.
            await _unitOfWork.PurchaseReceipt.CreateAsync(purchaseReceipt);
            await _unitOfWork.SaveAsync();

            // Get the created receipt ID.
            int purchaseReceiptId = purchaseReceipt.Id;

            // Assign the created receipt ID to each receipt item adn create item ledger entry for each item
            var itemLedgerList = new List<ItemLedger>();
            foreach (var receiptItem in purchaseReceiptItems)
            {
                receiptItem.PurchaseReceiptId = purchaseReceiptId;

                //Add the transaction entry to ledger
                CreateItemLedgerEntry(itemLedgerList, receiptItem);
            }

            //Add item ledger entries to database
            await InsertItemLedgerEntryAsync(itemLedgerList);

            // Add the receipt items to the database.
            await _unitOfWork.PurchaseReceiptItem.AddRangeAsync(purchaseReceiptItems);
            await _unitOfWork.SaveAsync();

            // Return the created receipt ID.
            return purchaseReceiptId;
        }

        //  Method to create a new entry in the item ledger for a received item and adds it to the provided list.
        private static void CreateItemLedgerEntry(List<ItemLedger> itemLedgerList, PurchaseReceiptItem receiptItem)
        {
            var itemLedger = new ItemLedger()
            {
                ItemId = receiptItem.ItemId,
                Quantity = receiptItem.ReceivedQuantity,
                TransactionDate = DateTime.Now,
                TransactionType = SD.PurchaseReceipt_TransactionType
            };
            itemLedgerList.Add(itemLedger);
        }

        // Method to validate receipt items and checks their quantities against the purchase order.
        private async Task<List<PurchaseReceiptItem>> ValidateReceiptItem(PurchaseReceiptCreateDTO createDTO, PurchaseOrder purchaseOrder)
        {
            var purchaseReceiptItems = new List<PurchaseReceiptItem>();

            foreach (var receiptItem in createDTO.ReceiptItems)
            {
                // Retrieve the item details.
                Item item = await _unitOfWork.Item.GetAsync(i => i.Id == receiptItem.ItemId);

                if (item == null)
                {
                    throw new ApiNotFoundException(new List<string> { $"Item with ID {receiptItem.ItemId} not found." });
                }

                // Check if the item is part of the original purchase order.
                bool isItemInOrder = purchaseOrder.OrderItems.Any(i => i.ItemId == receiptItem.ItemId);
                if (!isItemInOrder)
                {
                    throw new ApiNotFoundException(new List<string> { $"Item with ID {receiptItem.ItemId} is not found in the original purchase order." });
                }

                // Get the quantity ordered for the item.
                int PurchaseOrderItemQuantity = purchaseOrder.OrderItems
                    .Where(u => u.ItemId == receiptItem.ItemId)
                    .FirstOrDefault()
                    .Quantity;

                // Retrieve the existing purchase receipt and calculate the total received quantity for the item.
                List<PurchaseReceipt> receiptList = await _unitOfWork.PurchaseReceipt.GetAllAsync(u => u.PurchaseOrderId == createDTO.PurchaseOrderId, "ReceiptItems");

                int ReceiptOrderItemQuantity = 0;
                if (receiptList.Any())
                {
                    ReceiptOrderItemQuantity = receiptList
                        .SelectMany(receipt => receipt.ReceiptItems)
                        .Where(item => item.ItemId == receiptItem.ItemId)
                        .Sum(item => item.ReceivedQuantity);
                }

                // Validate if the received quantity exceeds the ordered quantity.
                int remainingQuantity = PurchaseOrderItemQuantity - ReceiptOrderItemQuantity;
                if (receiptItem.ReceivedQuantity > remainingQuantity)
                {
                    throw new ApiBadRequestException(new List<string> { $"Received quantity exceeds ordered remaing quantity ({remainingQuantity}) for item {receiptItem.ItemId}" });
                }

                // Update item inventory and reservation quantities.
                HandleItemInventoryUpdates(receiptItem, item);

                // Add the validated receipt item to the list.
                purchaseReceiptItems.Add(new PurchaseReceiptItem
                {
                    ItemId = receiptItem.ItemId.Value,
                    ReceivedQuantity = receiptItem.ReceivedQuantity.Value
                });
            }

            return purchaseReceiptItems;
        }

        // Method to update the inventory and reserved quantities for an item.
        private static void HandleItemInventoryUpdates(PurchaseReceiptItemCreateDTO receiptItem, Item item)
        {
            item.Quantity += receiptItem.ReceivedQuantity.Value;
        }

        // Method to validate the purchase order for the receipt and ensures the purchase order exists and has the correct status.
        private async Task<PurchaseOrder> ValidatePurchaseReceipt(PurchaseReceiptCreateDTO createDTO)
        {
            if (createDTO.ReceiptItems == null || !createDTO.ReceiptItems.Any())
            {
                throw new ApiBadRequestException(new List<string> { "The receiptItems array must contain at least one item." });
            }

            PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetAsync(po => po.Id == createDTO.PurchaseOrderId, "OrderItems");
            if (purchaseOrder == null || purchaseOrder.Status != SD.PO_Status_Completed)
            {
                throw new ApiNotFoundException(new List<string> { "Purchase order not found or not completed." });
            }

            // Check for duplicate items in the order.
            var duplicateItems = createDTO.ReceiptItems
                .GroupBy(i => i.ItemId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateItems.Any())
            {
                throw new ApiBadRequestException(new List<string> { $"Duplicate items found: {string.Join(", ", duplicateItems)}." });
            }

            // Return the validated purchase order.
            return purchaseOrder;
        }

        // Method to map the PurchaseReceipt entity to PurchaseReceiptDTO, including receipt item details.
        private async Task<PurchaseReceiptDTO> MapToPurchaseReceiptDTO(PurchaseReceipt purchaseReceipt)
        {
            // Initialize a new PurchaseReceiptDTO object to hold the mapped data.
            var purchaseReceiptDTO = new PurchaseReceiptDTO();

            // Loop through each ReceiptItem in the PurchaseReceipt.
            foreach (var Item in purchaseReceipt.ReceiptItems)
            {
                // Create a new DTO for the ReceiptItem.
                var purchaseReceiptItemDTO = new PurchaseReceiptItemDTO
                {
                    ItemId = Item.ItemId,
                    ReceivedQuantity = Item.ReceivedQuantity
                };

                // Fetch the Item entity for the current ReceiptItem using its ItemId.
                Item item = await _unitOfWork.Item.GetAsync(i => i.Id == Item.ItemId);

                // Map the item's name to the DTO.
                purchaseReceiptItemDTO.ItemName = item.Name;

                // Add the PurchaseReceiptItemDTO to the PurchaseReceiptDTO.
                purchaseReceiptDTO.ReceiptItems.Add(purchaseReceiptItemDTO);
            }
            // Return the mapped PurchaseReceiptDTO.
            return purchaseReceiptDTO;
        }

        // Method to retrieve a PurchaseReceipt from the database by its ID.
        private async Task<PurchaseReceipt> GetPurchaseReceiptAsync(int id)
        {
            // Fetch the PurchaseReceipt from the database along with its related ReceiptItems.
            PurchaseReceipt purchaseReceipt = await _unitOfWork.PurchaseReceipt.GetAsync(u => u.Id == id, "ReceiptItems");

            // If the PurchaseReceipt is not found, throw an ApiNotFoundException.
            if (purchaseReceipt == null)
            {
                throw new ApiNotFoundException(new List<string> { "Purchase Receipt ID not found." });
            }

            // Return the found PurchaseReceipt entity.
            return purchaseReceipt;
        }

        // Method to validate that the PurchaseReceipt ID is valid (must be greater than 0).
        private static void ValidatePurchaseReceiptId(int id)
        {
            if (id <= 0)
            {
                throw new ApiBadRequestException(new List<string> { "Invalid Purchase Receipt ID." });
            }
        }

        //Method to insert a list of item ledger entries into the database.
        private async Task InsertItemLedgerEntryAsync(List<ItemLedger> itemLedgers)
        {
            await _unitOfWork.ItemLedger.AddRangeAsync(itemLedgers);
        }
    }
}
