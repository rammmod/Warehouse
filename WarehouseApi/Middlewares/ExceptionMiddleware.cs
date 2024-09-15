using FluentValidation;
using WarehouseApi.Exceptions.Base;

namespace WarehouseApi.Middlewares
{
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
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is HttpResponseException exception)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = exception.StatusCode;

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = exception.StatusCode,
                        Message = exception.Message
                    }.ToString());
                }
                else if (ex is ValidationException validationException)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = validationException.Message
                    }.ToString());
                }
                else
                    throw;
            }
        }
    }
}
