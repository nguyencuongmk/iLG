using iLG.API.Extensions;
using iLG.API.Models.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace iLG.API.Handlers
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"Error Message: {exception.Message}, Time of occurrence {DateTime.UtcNow}");

            var problemDetails = new ProblemDetails
            {
                Title = exception.GetType().Name,
                Instance = context.Request.Path
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            var response = new ApiResponse();

            response.Errors.Add(new Error
            {
                ErrorMessage = exception.Message,
                ErrorDetail = problemDetails
            });

            var result = response.GetResult(context.Response.StatusCode, "An Exception occurred");

            await context.Response.WriteAsJsonAsync(result, cancellationToken);
            return true;
        }
    }
}