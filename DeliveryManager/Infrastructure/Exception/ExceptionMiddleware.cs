using DeliveryManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryManager.Infrastructure.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isDevelopment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, bool isDevelopment = false)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _isDevelopment = isDevelopment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, nameof(ValidationException));
                await WriteErrorResponse(context, ex.StatusCode, ex.ErrorCodes, ex.Message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                string errorResponse;

                if (_isDevelopment)
                {
                    errorResponse = $"{ex.Message}\n{ex.StackTrace}";
                    if (ex.InnerException != null)
                        errorResponse += $"\n{ex.InnerException.Message}\n{ex.InnerException.StackTrace}";
                }
                else
                {
                    errorResponse = "An error has occured.";
                }

                await WriteErrorResponse(context, HttpStatusCode.InternalServerError,
                    ErrorCodes.InternalError, errorResponse);
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, ErrorCodes errorCode, string message)
        {
            context.Response.Clear();

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            var response = JsonConvert.SerializeObject(new ErrorResource
            {
                Code = errorCode.ToString(),
                Message = message
            });

            await context.Response.WriteAsync(response);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder, bool isDevelopment = false)
        {
            return builder.UseMiddleware<ExceptionMiddleware>(isDevelopment);
        }
    }
}
