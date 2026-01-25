using SurvayBacket.Api.Services;

namespace SurvayBacket.Api.Middlewares
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IOperationSingleton _operationSingleton;
        public MyMiddleware(RequestDelegate requestDelegate, ILogger<MyMiddleware> logger, IOperationSingleton operationSingleton)
        {
            _next = requestDelegate;
            _logger = logger;
            _operationSingleton = operationSingleton;
        }

        public async Task InvokeAsync(HttpContext httpContext 
            ,IOperationScoped operationScoped
            ,IOperationTransient operationTransient)
        {
            _logger.LogInformation("Transient{0}", operationTransient.OpetationId);
            _logger.LogWarning("Scoped{0}", operationScoped.OpetationId);
            _logger.LogInformation("Singlton{0}", _operationSingleton.OpetationId);
            await _next(httpContext);
        }
    }
}
