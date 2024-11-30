using log4net;

namespace API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<CorrelationIdMiddleware> logger)
        {
            // Check for an existing correlation ID in the request
            var correlationId = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            // Set the correlation ID in HttpContext for use in the application
            httpContext.Items["CorrelationId"] = correlationId;
            LogicalThreadContext.Properties["CorrelationId"] = correlationId;

            // Use logging scope to include the correlation ID in all log entries
            using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                await _next(httpContext);
            }
        }
    }
}
