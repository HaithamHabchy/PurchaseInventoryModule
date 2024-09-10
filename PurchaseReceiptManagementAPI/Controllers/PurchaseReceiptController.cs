using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using PurchaseReceiptManagementAPI.Services.IServices;

namespace PurchaseReceiptManagementAPI.Controllers
{
    // Controller for purchase receipt operations.
    [Route("api/purchase-receipt")]
    [ApiController]
    public class PurchaseReceiptController : ControllerBase
    {
        private readonly IPurchaseReceiptService _purchaseReceiptService;

        public PurchaseReceiptController(IPurchaseReceiptService purchaseReceiptService)
        {
            _purchaseReceiptService = purchaseReceiptService;
        }

        // Creates a new purchase receipt based on the provided DTO.
        // Returns an ApiResponse containing the result of the creation operation.
        // Responds with appropriate status codes and error messages based on the outcome.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> CreatePurchaseReceipt([FromBody] PurchaseReceiptCreateDTO createDTO)
        {
            try
            {
                var response = new ApiResponse
                {
                    Result = await _purchaseReceiptService.CreatePurchaseReceiptAsync(createDTO),
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

        // Retrieves purchase receipt details by its ID.
        // Returns an ApiResponse containing the purchase receipt details if found.
        // Responds with appropriate status codes and error messages based on the outcome.
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> GetPurchaseReceipt(int id)
        {
            try
            {
                var response = new ApiResponse
                {
                    Result = await _purchaseReceiptService.GetPurchaseReceiptByIdAsync(id),
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
