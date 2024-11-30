using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            var correlationId = context.Items["CorrelationId"]?.ToString();
            var logger = context.RequestServices.GetService<ILogger<GlobalExceptionHandlingMiddleware>>();
            logger?.LogError(ex, "An error occurred. CorrelationId: {CorrelationId}", correlationId);

            // Return a structured error response
            var errorResponse = new
            {
                CorrelationId = correlationId,
                Message = "An unexpected error occurred. Please contact support with the provided Correlation ID.",
                Details = ex.Message // Avoid exposing sensitive details in production
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
