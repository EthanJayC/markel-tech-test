using Microsoft.AspNetCore.Mvc;

namespace Markel.Api.Security;

public sealed class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAnonymousPath(context.Request.Path))
        {
            await next(context);
            return;
        }

        var expected = configuration[ApiKeyDefaults.ConfigurationKey];
        if (string.IsNullOrWhiteSpace(expected)
            || !context.Request.Headers.TryGetValue(ApiKeyDefaults.HeaderName, out var provided)
            || !string.Equals(provided.ToString(), expected, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = $"A valid {ApiKeyDefaults.HeaderName} header is required."
            });
            return;
        }

        await next(context);
    }

    private static bool IsAnonymousPath(PathString path) =>
        path.StartsWithSegments("/swagger")
        || path.StartsWithSegments("/favicon.ico");
}
