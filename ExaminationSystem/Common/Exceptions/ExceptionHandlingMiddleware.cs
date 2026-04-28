using System.Text.Json;

namespace ExaminationSystem.Common.Exceptions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Not found: {Message}", ex.Message);
                context.Response.StatusCode = 404;
                await WriteJson(context, new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                logger.LogWarning(ex, "Conflict: {Message}", ex.Message);
                context.Response.StatusCode = 409;
                await WriteJson(context, new
                {
                    message = ex.Message,
                    existing_result = ex.ExistingResult
                });
            }
            catch (ForbiddenAccessException ex)
            {
                logger.LogWarning(ex, "Forbidden: {Message}", ex.Message);
                context.Response.StatusCode = 403;
                await WriteJson(context, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = 500;
                await WriteJson(context, new { message = "An unexpected error occurred." });
            }
        }

        private static Task WriteJson(HttpContext ctx, object body)
        {
            ctx.Response.ContentType = "application/json";
            return ctx.Response.WriteAsync(
                JsonSerializer.Serialize(body, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
        }
    }
}
