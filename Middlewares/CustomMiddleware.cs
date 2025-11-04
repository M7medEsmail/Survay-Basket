namespace SurvayBacket.Api.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomMiddleware(ILogger<CustomMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            _logger.LogInformation("Processing Request!!!!! ");
            await _next(httpContext);
            _logger.LogWarning("Processing Respond After Hit End Point");
        }
    }
}
