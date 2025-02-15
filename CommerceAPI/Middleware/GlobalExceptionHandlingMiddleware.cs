using API.Exceptions;
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
            var env = context.RequestServices.GetService<IWebHostEnvironment>();

            logger?.LogError(ex, "An error occurred. CorrelationId: {CorrelationId}", correlationId);

            int statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorResponse = new ErrorResponse
            {
                CorrelationId = correlationId,
                StatusCode = statusCode,
                Message = ex switch
                {
                    NotFoundException => ex.Message,
                    ValidationException => ex.Message,
                    _ => "An unexpected error occurred. Please contact support with the provided Correlation ID."
                },
                StackTrace = env.IsDevelopment() ? ex.StackTrace : null // Only include stack trace in Development
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
