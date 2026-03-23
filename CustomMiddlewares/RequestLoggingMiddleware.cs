namespace WebApplication4.CustomMiddlewares
{
    public class RequestLoggingMiddleware 
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next, 
            ILogger<RequestLoggingMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            _logger.LogInformation("Request {Method} {Path} {Time}",
                                    request.Method,
                                    request.Path,
                                    DateTime.UtcNow);

                await _next(context);
        }
    }
}
