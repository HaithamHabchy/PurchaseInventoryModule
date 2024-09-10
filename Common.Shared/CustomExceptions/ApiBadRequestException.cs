namespace Common.Shared.CustomExceptions
{
    public class ApiNotFoundException : Exception
    {
        public List<string> ErrorMessages { get; set; } = new();
        public ApiNotFoundException(IEnumerable<string> errorMessages) : base("Validation failed.")
        {
            ErrorMessages.AddRange(errorMessages);
        }
    }
}
