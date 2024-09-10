namespace Common.Shared.DTOs
{
    // Represents a standardized response for API requests.
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object? Result { get; set; }
    }
}
