using HR.LeaveManagement.API.Models;
using HR.LeaveManagement.Application.Exceptions;
using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace HR.LeaveManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            CustomValidationProblemDetails problemDetails = new();

            switch (ex)
            {
                case BadRequestExceptions badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problemDetails = new CustomValidationProblemDetails
                    {
                        Title = badRequestException.Message,
                        Status = (int)statusCode,
                        Detail = badRequestException.InnerException?.Message,
                        Type = nameof(badRequestException),
                        Errors = badRequestException.ValidationErrors
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    problemDetails = new CustomValidationProblemDetails
                    {
                        Title = notFoundException.Message,
                        Status = (int)statusCode,
                        Detail = notFoundException.InnerException?.Message,
                        Type = nameof(notFoundException)
                    };
                    break;

                default:
                    problemDetails = new CustomValidationProblemDetails
                    {
                        Title = ex.Message,
                        Status = (int)statusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        Detail = ex.StackTrace
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}