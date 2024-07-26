using System.Net;
using System.Text.Json;

namespace Ecommerce.Authentication.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var responseModel = new
            {
                error = exception.Message,
                stackTrace = exception.StackTrace
            };

            switch (exception)
            {
                case ApplicationException e:
                    // Custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // Not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ArgumentException e:
                    response.StatusCode= (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    // Unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            return response.WriteAsync(result);
        }
    }
}
