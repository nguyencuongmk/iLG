﻿using iLG.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace iLG.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read request body
            var requestBody = await ReadRequestBody(context.Request);

            _logger.WriteLog(LogLevel.Information, $"[START] Handle Request Method: {context.Request.Method} - Request Path: {context.Request.Path}", requestBody);

            // Set the response body into a Stream to log the response
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                var timer = new Stopwatch();
                timer.Start();

                // Handle request
                await _next(context);

                timer.Stop();
                var timeTaken = timer.Elapsed;
                if (timeTaken.Seconds > 3)
                    _logger.WriteLog(LogLevel.Warning, $"[PERFORMANCE] The request {context.Request.Method} took {timeTaken.Seconds}s", JsonConvert.SerializeObject(new { requestUrl = context.Request.GetDisplayUrl() }, Formatting.Indented));

                // Read response body
                var responseBodyContent = await ReadResponseBody(context.Response);

                // Log response information
                _logger.WriteLog(LogLevel.Information, $"[END] Handled {context.Request.Method} with Response Status Code: {context.Response.StatusCode}", responseBodyContent);

                // Reset response body
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return requestBody;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return string.IsNullOrEmpty(responseBody) ? responseBody : JToken.Parse(responseBody).ToString(Formatting.Indented);
        }
    }
}