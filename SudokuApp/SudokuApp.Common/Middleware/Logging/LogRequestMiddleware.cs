using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SudokuApp.Common.Middleware.Logging
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LogRequestMiddleware(RequestDelegate next, ILoggerFactory factory)
        {
            _next = next;
            _logger = factory.CreateLogger(nameof(SudokuApp));
        }

        public async Task Invoke(HttpContext context)
        {
            var stream = context.Request.Body;
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            buffer.Seek(0, SeekOrigin.Begin);

            var requestBodyText = await new StreamReader(buffer).ReadToEndAsync();
            buffer.Seek(0, SeekOrigin.Begin);

            _logger.LogRequest(context, requestBodyText);
            context.Request.Body = buffer;

            await _next(context);

            context.Request.Body = stream;
        }
    }
}