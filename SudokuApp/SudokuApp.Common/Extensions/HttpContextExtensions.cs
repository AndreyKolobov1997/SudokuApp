using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SudokuApp.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetIp(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var remoteIpAddress = httpContext.Connection?.RemoteIpAddress?.MapToIPv4(); //TODO: IPv6?

            var headers = httpContext.Request?.Headers;
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For
            if (headers != null)
            {
                ip = GetHeaderValue(headers, "X-Forwarded-For");
            }

            if (ip.IsNullOrWhiteSpace() && remoteIpAddress != null)
            {
                ip = remoteIpAddress.ToString();
            }

            if (ip.IsNullOrWhiteSpace() && headers != null)
            {
                ip = GetHeaderValue(headers, "REMOTE_ADDR");
            }

            return ip;
        }

        private static string GetHeaderValue(IHeaderDictionary headers, string headerName)
        {
            var values = headers[headerName];
            return values == StringValues.Empty ? null : values.First().Split(',').FirstOrDefault();
        }

        public static bool IsApiRequest(this HttpContext httpContext)
        {
            return Regex.IsMatch(httpContext.Request.Path.ToString(), @"^[a-zA-Z0-9\/]+\/api\/.*$");
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}