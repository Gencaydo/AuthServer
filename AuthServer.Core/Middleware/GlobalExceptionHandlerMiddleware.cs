using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
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

            var errorResponse = Response<NoDataDto>.Fail(
                "An unexpected error occurred. Please try again later.", 500, true);

            var errorJson = JsonSerializer.Serialize(errorResponse);

            var db = _redis.GetDatabase();
            var logKey = $"error:log:{Guid.NewGuid()}";
            await db.StringSetAsync(logKey, errorJson);

            await response.WriteAsync(errorJson);
        }
    }
}
