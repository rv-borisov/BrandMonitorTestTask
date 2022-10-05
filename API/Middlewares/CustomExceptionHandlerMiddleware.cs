using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = GetStatusCode(exception);

            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            ProblemDetails problemDetails = new();
            problemDetails.Detail = exception.Message;
            problemDetails.Status = (int)code;
            problemDetails.Instance = context.Request.Path;

            if (string.IsNullOrEmpty(problemDetails.Instance))
            {
                problemDetails.Detail = exception.Message;

                if (exception.InnerException != null)
                    problemDetails.Detail += "\n" + exception.InnerException.Message;
                problemDetails.Detail = problemDetails.Detail.Replace("\"", "");
                Console.Write(exception.StackTrace);
            }

            var result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                WrongDataFormatException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };
        }
    }
}
