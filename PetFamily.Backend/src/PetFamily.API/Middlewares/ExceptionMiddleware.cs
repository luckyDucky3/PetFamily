using System.Net.Mime;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); //вызов всего приложения
        }
        catch (Exception ex)
        {
            var error = Error.Failure("internal.server", ex.Message);
            Envelope envelope = Envelope.Error(error);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}