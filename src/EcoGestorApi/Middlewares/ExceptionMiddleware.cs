using System.Net;
using System.Text.Json;

namespace EcoGestorApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Erro não tratado na EcoGestor API");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex is InvalidOperationException ? 400 : (int)HttpStatusCode.InternalServerError;
            var response = new { mensagem = ex is InvalidOperationException ? ex.Message : "Ocorreu um erro interno na API." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
