using DataAccessLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SupplierManagementAPI.Filters
{
    /// Global exception filter to handle unexpected errors across the application.
    /// This filter catches any unhandled exceptions and returns a standardized error response.
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        /// Constructor to initialize the GlobalExceptionFilter with the hosting environment.
        public GlobalExceptionFilter([FromServices] IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        /// Handles an exception that occurred during the request execution.
        /// If the application is in a non-development environment, it hides detailed error information.
        /// Otherwise, it provides detailed exception data.
        public void OnException(ExceptionContext context)
        {
            var response = new ApiResponse
            {
                IsSuccess = false
            };

            // Set generic error messages for production or detailed ones for development.
            if (!_hostEnvironment.IsDevelopment())
            {
                response.ErrorMessages = new List<string> { "An unexpected error occurred." };

            }
            else
            {
                // Include full exception details in development mode.
                response.ErrorMessages = new List<string> { context.Exception.ToString() };
            }

            // Set the status code to 500 Internal Server Error and return the response object.
            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            // Prevent further propagation of the exception.
            context.ExceptionHandled = true;
        }
    }
}
