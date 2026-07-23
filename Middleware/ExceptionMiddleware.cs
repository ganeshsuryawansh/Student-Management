using StudentManagementSystem.Models.DTOs;
using System.Net;
using System.Text.Json;

namespace StudentManagementSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                AppException appEx => appEx.StatusCode,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            // Log with appropriate severity: expected app exceptions as warnings, everything else as errors.
            if (ex is AppException)
            {
                _logger.LogWarning(ex, "Handled application exception: {Message}", ex.Message);
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            }

            var message = ex is AppException
                ? ex.Message
                : "An unexpected error occurred. Please try again later.";

            // Include stack trace only in Development to avoid leaking internals in production.
            var response = ApiResponse<object>.Fail(message);
            if (_env.IsDevelopment() && ex is not AppException)
            {
                response.Data = new { ex.StackTrace };
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
