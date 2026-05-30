namespace FlightSearch.Api.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var timestamp = DateTimeOffset.UtcNow;
        var method = context.Request.Method;
        var path = context.Request.Path;

        await next(context);

        var status = context.Response.StatusCode;
        var level = status >= 500 ? LogLevel.Error
                  : status >= 400 ? LogLevel.Warning
                  : LogLevel.Information;

        logger.Log(level,
            "[{Timestamp:O}] {Method} {Path} → {StatusCode}",
            timestamp, method, path, status);
    }
}
