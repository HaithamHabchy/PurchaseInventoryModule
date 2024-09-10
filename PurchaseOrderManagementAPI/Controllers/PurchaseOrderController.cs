using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderManagementAPI.Services.IServices;

namespace PurchaseOrderManagementAPI.Controllers
{
    // Controller for purchase order operations.
    [Route("api/purchase-order")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        // Creates a new purchase order based on the provided DTO.
        // Returns an ApiResponse containing the result of the creation operation.
        // Responds with appropriate status codes and error messages based on the outcome.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> CreatePurchaseOrder([FromBody] PurchaseOrderCreateDTO createDTO)
        {
            try
            {
                var response = new ApiResponse
                {
                    Result = await _purchaseOrderService.CreatePurchaseOrderAsync(createDTO),
                    IsSuccess = true
                };
                return Ok(response);
            }
            catch (ApiBadRequestException ex)
            {
                var response = new ApiResponse
                {
                    ErrorMessages = ex.ErrorMessages,
                };
                return BadRequest(response);
            }
            catch (ApiNotFoundException ex)
            {
                var response = new ApiResponse
                {
                    ErrorMessages = ex.ErrorMessages,
                };
                return NotFound(response);
            }
        }

        // HTTP PATCH endpoint to update the status of a purchase order.
        // It accepts a PurchaseOrderStatusUpdateDTO as input and returns an ApiResponse.
        [HttpPatch("update-status")]
        public async Task<ActionResult<ApiResponse>> UpdatePurchaseOrderStatus([FromBody] PurchaseOrderStatusUpdateDTO updateDTO)
        {
            try
            {
                await _purchaseOrderService.UpdatePurchaseOrderStatusAsync(updateDTO);
                var response = new ApiResponse
                {
                    IsSuccess = true
                };
                return Ok(response);
            }
            catch (ApiBadRequestException ex)
            {
                var response = new ApiResponse
                {
                    ErrorMessages = ex.ErrorMessages,
                };
                return BadRequest(response);
            }
            catch (ApiNotFoundException ex)
            {
                var response = new ApiResponse
                {
                    ErrorMessages = ex.ErrorMessages,
                };
                return NotFound(response);
            }
        }
    }
}
