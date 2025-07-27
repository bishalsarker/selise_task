namespace SeliseTaskManager.Infrastructure.Common.DTO
{
    public class ApiExceptionResponse
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string Details { get; }

        public ApiExceptionResponse(int statusCode, string message, string? details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details ?? string.Empty;
        }
    }
}
