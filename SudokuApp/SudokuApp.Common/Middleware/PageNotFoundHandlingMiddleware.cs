using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SudokuApp.Common.Configuration.Options;
using SudokuApp.Common.Extensions;

namespace SudokuApp.Common.Middleware
{
    public class PageNotFoundHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        private readonly PageNotFoundHandlingMiddlewareOptions _options;

        public PageNotFoundHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<PageNotFoundHandlingMiddlewareOptions> options)
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

            if (httpContext.Response.StatusCode != (int)HttpStatusCode.NotFound || httpContext.IsApiRequest())
            {
                return;
            }

            var redirectPath = httpContext.Request.PathBase +
                               _options.RedirectRelativePath +
                               httpContext.Request.QueryString;

            var from = httpContext.Request.Path;
            _loggerFactory.CreateLogger<PageNotFoundHandlingMiddleware>().LogWarning(
                $"404 Not Found: Redirecting from: {from} to: {redirectPath}");

            if (from == redirectPath)
            {
                return;
            }
            httpContext.Response.Redirect(redirectPath);
        }
    }

    public static class PageNotFoundHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UsePageNotFoundHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PageNotFoundHandlingMiddleware>();
        }
    }
}
