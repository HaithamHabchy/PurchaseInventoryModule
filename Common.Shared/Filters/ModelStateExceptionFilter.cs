using Common.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Shared.Filters
{
    // Filter to handle invalid model state and modify the default error message format.
    // Converts validation errors into a standardized ApiResponse.
    public class ModelStateExceptionFilter : IAsyncResultFilter
    {
        // Executes the filter to handle invalid model state.
        // If the model state is invalid, it modifies the default error message format
        // and returns a unified ApiResponse containing the error messages.
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var httpContext = context.HttpContext;

            // Check if the model state is invalid.
            if (!context.ModelState.IsValid)
            {
                // Extract error messages from the model state.
                var errorMessages = context.ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                // Create a unified ApiResponse with the error messages.
                var response = new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessages = errorMessages
                };

                // Set the result as a BadRequestObjectResult with the ApiResponse.
                context.Result = new BadRequestObjectResult(response);
            }

            // Set the result as a BadRequestObjectResult with the ApiResponse.
            await next();
        }
    }
}
