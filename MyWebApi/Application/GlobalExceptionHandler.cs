using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            NotFoundException =>
                (StatusCodes.Status404NotFound, "Запис не знайдено"),

            ValidationException =>
                (StatusCodes.Status400BadRequest, "Помилка валідації"),

            BusinessRuleException =>
                (StatusCodes.Status400BadRequest, "Порушено бізнес-правило"),

            _ =>
                (StatusCodes.Status500InternalServerError, "Внутрішня помилка сервера")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Необроблена помилка");

        context.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status == StatusCodes.Status500InternalServerError
                ? "Сталася неочікувана помилка."
                : exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}