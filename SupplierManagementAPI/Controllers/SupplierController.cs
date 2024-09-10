using Common.Shared.CustomExceptions;
using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using SupplierManagementAPI.Services.IServices;

namespace SupplierManagementAPI.Controllers
{
    // Controller for managing supplier operations.
    [Route("api/supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        // Constructor to initialize the SupplierController with the ISupplierService dependency.
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // Retrieves a supplier by its ID.
        // Responds with 200 OK if the supplier is found, 400 Bad Request if the ID is invalid,
        // and 404 Not Found if the supplier is not found.
        [HttpGet("{id:int}", Name = "GetSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> GetSupplier(int id)
        {
            try
            {
                var response = new ApiResponse
                {
                    Result = await _supplierService.GetSupplierByIdAsync(id),
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


        // Creates a new supplier.
        // Responds with 201 Created and the created supplier's details on success,
        // and 400 Bad Request if there are validation errors.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> CreateSupplier([FromBody] SupplierCreateDTO createDTO)
        {
            try
            {
                SupplierDTO supplierDTO = await _supplierService.CreateSupplierAsync(createDTO);
                var response = new ApiResponse
                {
                    Result = supplierDTO,
                    IsSuccess = true
                };
                return CreatedAtRoute("GetSupplier", new { id = supplierDTO.Id }, response);
            }
            catch (ApiBadRequestException ex)
            {
                var response = new ApiResponse
                {
                    ErrorMessages = ex.ErrorMessages,
                };
                return BadRequest(response);
            }
        }

        // Deletes a supplier by its ID.
        // Responds with 200 OK on successful deletion, 400 Bad Request if the ID is invalid,
        // and 404 Not Found if the supplier is not found.
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> DeleteSupplier(int id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
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

        // Updates an existing supplier.
        // Responds with 200 OK on successful update, 400 Bad Request if there are validation errors,
        // and 404 Not Found if the supplier is not found.
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ApiResponse>> UpdateSupplier(int id, [FromBody] SupplierUpdateDTO updateDTO)
        {
            try
            {
                await _supplierService.UpdateSupplierAsync(id, updateDTO);
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
