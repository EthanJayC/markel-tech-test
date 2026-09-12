using FluentValidation;
using Markel.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Markel.Api.Middleware;

public sealed class ExceptionHandlingMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title, detail, errors) = exception switch
        {
            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFound.Message,
                (IDictionary<string, string[]>?)null),
            ValidationException validation => (
                StatusCodes.Status400BadRequest,
                "Validation Failed",
                "One or more validation errors occurred.",
                validation.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray())),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Server Error",
                "An unexpected error occurred.",
                null)
        };

        httpContext.Response.StatusCode = status;

        if (errors is not null)
        {
            await httpContext.Response.WriteAsJsonAsync(new HttpValidationProblemDetails(errors)
            {
                Status = status,
                Title = title,
                Detail = detail
            }, cancellationToken);
            return true;
        }

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        }, cancellationToken);

        return true;
    }
}
