using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace AuthServer.Core.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new
            {
                StatusCode = response.StatusCode,
                Message = "An unexpected error occurred. Please try again later.",
                Detailed = exception.Message,
                Timestamp = DateTime.UtcNow
            };

            // Fix for CS1501 and CS0815: Use System.Text.Json.JsonSerializer.Serialize with the correct overload
            var errorJson = JsonSerializer.Serialize(errorDetails);

            var db = _redis.GetDatabase();
            var logKey = $"error:log:{Guid.NewGuid()}";
            await db.StringSetAsync(logKey, errorJson);

            await response.WriteAsync(errorJson);
        }
    }
}
