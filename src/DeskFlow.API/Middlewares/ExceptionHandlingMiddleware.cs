using System.Text.Json;
namespace DeskFlow.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
     private readonly RequestDelegate _next;
     public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }   
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception exception)
            {
              await TratarExcecaoAsync( context, exception);  
            }
        }
        private static async Task TratarExcecaoAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
              ArgumentException => StatusCodes.Status400BadRequest, InvalidOperationException => StatusCodes.Status400BadRequest,
              KeyNotFoundException => StatusCodes.Status404NotFound,UnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
              _=> StatusCodes.Status500InternalServerError  
            };

            var resposta = new
            {
                status = statusCode,
                message = exception.Message
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
        }
    }
}