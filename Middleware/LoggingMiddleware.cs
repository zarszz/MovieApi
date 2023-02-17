using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieAPi.Entities;

namespace MovieAPi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestLog> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<HttpRequestLog> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseContext databaseContext)
        {
            var request = await FormatRequest(context.Request);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var response = await FormatResponse(context.Response);

                var log = new HttpRequestLog
                {
                    RequestPath = context.Request.Path,
                    RequestMethod = context.Request.Method,
                    RequestBody = request,
                    ResponseBody = response,
                    ResponseStatusCode = context.Response.StatusCode,
                    RequestTime = DateTime.UtcNow
                };
                // Log information about the request before it is processed
                _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path}{context.Request.QueryString} with body '{request}' received.");

                // Log information about the response after it is processed
                _logger.LogInformation($"Response with status code {context.Response.StatusCode} and body '{responseBody}' returned.");

                await databaseContext.HttpRequestLogs.AddAsync(log);
                await databaseContext.SaveChangesAsync();

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return $"{request.Scheme} {request.Method} {request.Host}{request.Path} {request.QueryString} {body}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
    
}