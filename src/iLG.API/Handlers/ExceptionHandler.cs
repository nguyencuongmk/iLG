using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.Infrastructure.Extentions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace iLG.API.Handlers
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.WriteLog(LogLevel.Error, $"[Exception] {exception.Message}", JsonConvert.SerializeObject(new { errorMessage = exception.Message, requestUrl = context.Request.GetDisplayUrl(), timeStamp = DateTime.UtcNow }, Formatting.Indented));

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