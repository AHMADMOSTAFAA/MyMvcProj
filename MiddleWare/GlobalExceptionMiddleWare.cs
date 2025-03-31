namespace WebApplication2.MiddleWare
{
    public class GlobalExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        public GlobalExceptionMiddleWare(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
           
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) { 
            _logger.LogError(ex,"unhandled Exception");
                await HandleExceptionAsync(context, ex);
            }

        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

          
            var response = new
            {
                success = false,
                message = "An unexpected error occurred.",
                detail = exception.Message 
            };

            
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
