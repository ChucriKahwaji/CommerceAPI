using API.Exceptions;
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
            var correlationId = context.Items["CorrelationId"]?.ToString();
            var logger = context.RequestServices.GetService<ILogger<GlobalExceptionHandlingMiddleware>>();
            logger?.LogError(ex, "An error occurred. CorrelationId: {CorrelationId}", correlationId);

            int statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new
            {
                CorrelationId = correlationId,
                StatusCode = statusCode,
                Message = ex switch
                {
                    NotFoundException => ex.Message,
                    ValidationException => ex.Message,
                    _ => "An unexpected error occurred. Please contact support with the provided Correlation ID."
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
