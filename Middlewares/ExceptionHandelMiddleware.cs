namespace SurvayBacket.Api.Middlewares
{
    public class ExceptionHandelMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandelMiddleware> logger)
    {
        private readonly RequestDelegate _next = requestDelegate;
        private readonly ILogger<ExceptionHandelMiddleware> _logger = logger;


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "something went wrong {Message}.", ex.Message);
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error!",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
