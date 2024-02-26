using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Marketplace.WebApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {

        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case UserNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ItemNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case OrderNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case InvalidUpdateStatusCommandException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    _logger.LogError(ex, ex.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(new { message = ex?.Message });
            await response.WriteAsync(result);
        }
    }
}


// Extension method used to add the middleware to the HTTP request pipeline.
//public static class ExceptionHandlerMiddlewareExtensions
//{
//    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
//    }
//}
