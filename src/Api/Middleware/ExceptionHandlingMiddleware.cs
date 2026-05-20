using System.Net;
using System.Text.Json;
using FluentValidation;
using MenuNews.Domain.Common;

namespace MenuNews.Api.Middleware;

//Global Exception Middleware - bắt mọi exception.
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            await WriteResponse(context, HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (KeyNotFoundException ex)
        {
            await WriteResponse(context, HttpStatusCode.NotFound, ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("Đã xảy ra lỗi hệ thống."));
        }
    }

    private static async Task WriteResponse(HttpContext context, HttpStatusCode status, ApiResponse<object> response)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
