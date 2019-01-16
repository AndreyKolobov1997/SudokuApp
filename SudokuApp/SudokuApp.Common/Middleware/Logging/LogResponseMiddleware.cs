using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SudokuApp.Common.Middleware.Logging
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LogResponseMiddleware(RequestDelegate next, ILoggerFactory factory)
        {
            _next = next;
            _logger = factory.CreateLogger(nameof(SudokuApp));
        }

        public async Task Invoke(HttpContext context)
        {
            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                var timer = Stopwatch.StartNew();

                await _next.Invoke(context);

                timer.Stop();

                buffer.Seek(0, SeekOrigin.Begin);

                using (var bufferReader = new StreamReader(buffer))
                {
                    var body = await bufferReader.ReadToEndAsync();
                    buffer.Seek(0, SeekOrigin.Begin);

                    if (!string.IsNullOrEmpty(body))
                    {
                        await buffer.CopyToAsync(stream);
                        context.Response.Body = stream;
                    }

                    _logger.LogResponse(context, body, timer.ElapsedMilliseconds);
                }
            }
        }
    }
}