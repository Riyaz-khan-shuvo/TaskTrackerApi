using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TaskTracker.Shared.Exceptions;
using TaskTracker.Shared.Models;

namespace TaskTracker.Shared.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                // Centralized logging (Serilog also works here)
                _logger.LogError(ex, "Unhandled exception occurred while processing {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred. Please contact support.";

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.UnprocessableEntity;
                    // message = JsonConvert.SerializeObject(validationException.Failures);
                    message = validationException.Message;
                    break;
                case DuplicateValueException duplicateValueException:
                    statusCode = HttpStatusCode.Conflict;
                    message = duplicateValueException.Message;
                    break;
                case AppDuplicateValueException appDuplicateValueException:
                    statusCode = HttpStatusCode.Conflict;
                    message = appDuplicateValueException.Message;
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;

                case ForbiddenSubsciptionException forbiddenSubscriptionException:
                    statusCode = HttpStatusCode.PaymentRequired;
                    message = "The page or resource you were trying to reach is absolutely forbidden for not paying.";
                    break;
                case ForbidResultException authenticationException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = "The page or resource you were trying to reach is absolutely forbidden for some reason.";
                    break;
                case AppGeneralValueException appGeneralValueException:
                    statusCode = HttpStatusCode.NotModified;
                    message = appGeneralValueException.Message;
                    break;
                case PreRequisiteException appGeneralValueException:
                    statusCode = HttpStatusCode.FailedDependency;
                    message = appGeneralValueException.Message;
                    break;
                case ArgumentNullException argumentNullException:
                    statusCode = HttpStatusCode.FailedDependency;
                    message = $"{argumentNullException.Message}";
                    break;
                case ArgumentException argumentException:
                    statusCode = HttpStatusCode.FailedDependency;
                    message = $"{argumentException.Message}";
                    break;
            }

            var response = new ResultDto
            {
                Status = "Fail",
                Message = message,
#if DEBUG
                ExMessage = exception.Message
#endif
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }


    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
