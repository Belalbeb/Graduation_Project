namespace Graduation_Project.Responses
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string? Details { get; set; }
    }
}