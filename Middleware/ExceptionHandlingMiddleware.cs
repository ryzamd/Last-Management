using LastManagement.Constants;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Text.Json;

namespace LastManagement.Middleware;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, AppConstants.ErrorMessages.Unauthorized),
            InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, AppConstants.ErrorMessages.NotFound),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            DbUpdateException dbEx => HandleDbUpdateException(dbEx),
            _ => (HttpStatusCode.InternalServerError, AppConstants.ErrorMessages.InternalError)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message = message,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private (HttpStatusCode, string) HandleDbUpdateException(DbUpdateException dbEx)
    {
        if (dbEx.InnerException is PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                "23505" => (HttpStatusCode.Conflict, "A record with this value already exists"),
                "23503" => (HttpStatusCode.Conflict, AppConstants.ErrorMessages.CannotDeleteHasRelations),
                "23502" => (HttpStatusCode.BadRequest, "Required field is missing"),
                _ => (HttpStatusCode.InternalServerError, AppConstants.ErrorMessages.InternalError)
            };
        }

        return (HttpStatusCode.InternalServerError, AppConstants.ErrorMessages.InternalError);
    }
}