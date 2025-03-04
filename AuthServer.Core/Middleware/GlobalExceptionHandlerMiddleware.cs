using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace AuthServer.Core.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Continue processing the request
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex); // Handle any unhandled exceptions
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            // Default to InternalServerError
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Base case for Status Codes and Messages
            var errorResponse = new
            {
                Code = context.Response.StatusCode, // HTTP status code
                Message = GetMessageForStatusCode(context.Response.StatusCode) // Dynamic message based on status code
            };

            // Return the error response as JSON
            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }

        // Helper method to return a message based on the status code
        private string GetMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.BadRequest => "The request could not be understood by the server due to malformed syntax.",
                (int)HttpStatusCode.Unauthorized => "The request requires user authentication.",
                (int)HttpStatusCode.Forbidden => "The server understood the request, but it refuses to authorize it.",
                (int)HttpStatusCode.NotFound => "The server has not found anything matching the request URI.",
                (int)HttpStatusCode.MethodNotAllowed => "The method specified in the request is not allowed for the resource identified by the request URI.",
                (int)HttpStatusCode.RequestTimeout => "The server timed out waiting for the request.",
                (int)HttpStatusCode.InternalServerError => "The server encountered an internal error and was unable to complete the request.",
                (int)HttpStatusCode.ServiceUnavailable => "The server is currently unable to handle the request due to a temporary overloading or maintenance of the server.",
                _ => "An unexpected error occurred. Please try again later." // Default case for unhandled status codes
            };
        }
    }
}
