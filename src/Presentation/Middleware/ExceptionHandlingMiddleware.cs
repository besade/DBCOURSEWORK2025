using Shop.Application.Exceptions;
using Shop.Domain.Exceptions;
using System.Net;

namespace Shop.Presentation.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DomainValidationException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (DomainNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await HandleExceptionAsync(context, "Внутрішня помилка сервера", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode code)
        {
            bool isApiRequest = context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                                context.Request.Path.StartsWithSegments("/api");

            if (isApiRequest)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                var result = System.Text.Json.JsonSerializer.Serialize(new { error = message });
                await context.Response.WriteAsync(result);
            }
            else
            {
                context.Response.StatusCode = (int)code;
                context.Response.ContentType = "text/html; charset=utf-8";

                context.Response.Redirect($"/Home/Error?message={Uri.EscapeDataString(message)}");
            }
        }
    }
}
