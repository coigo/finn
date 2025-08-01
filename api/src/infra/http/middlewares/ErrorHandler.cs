using System.Text.Json;

namespace Infra.Http.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)


        {
            int statusCode;
            string message;
            Console.WriteLine(ex.Source );
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            switch (ex)
            {
                case KeyNotFoundException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message ?? "Não encontrado.";
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message =  ex.Message ?? "Erro Interno.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message
            };

            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}