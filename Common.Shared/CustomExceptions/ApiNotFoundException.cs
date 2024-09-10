namespace Common.Shared.CustomExceptions
{
    public class ApiBadRequestException : Exception
    {
        public List<string> ErrorMessages { get; set; } = new();
        public ApiBadRequestException(IEnumerable<string> errorMessages) : base("Validation failed.")
        {
            ErrorMessages.AddRange(errorMessages);
        }
    }
}
