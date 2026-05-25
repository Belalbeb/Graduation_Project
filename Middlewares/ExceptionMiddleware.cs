using Graduation_Project.Exceptions;
using Graduation_Project.Responses;
using System.Text.Json;

namespace Graduation_Project.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                _logger.LogWarning(ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex.StatusCode,
                    ex.Message,
                    ex.StackTrace
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    ex.Message
                );
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            int statusCode,
            string message,
            string? details = null)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = details
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}