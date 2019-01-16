using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using SudokuApp.Common.Extensions;
using UAParser;

namespace SudokuApp.Common.Middleware.Logging
{
    public static class ApiLoggingExtensions
    {
        private const int MaxHeaderLength = 1000;

        public static void LogRequest(this ILogger logger, HttpContext context, string requestBody)
        {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();

            LogMessage(
                logger,
                "ApiRequest",
                ipAddress,
                context,
                context.Request.Headers,
                requestBody,
                0);
        }

        public static void LogResponse(this ILogger logger, HttpContext context, string responseBody, long durationMilliseconds)
        {
            var ipAddress = context.GetIp();

            LogMessage(
                logger,
                "ApiResponse",
                ipAddress,
                context,
                context.Response.Headers,
                responseBody,
                context.Response.StatusCode,
                durationMilliseconds);
        }

        private static void LogMessage(
            ILogger logger,
            string messageType,
            string ipAddress,
            HttpContext context,
            IHeaderDictionary headers,
            string body,
            int statusCode,
            long durationMillis = long.MinValue)
        {
            var headersString = GetHeaderTraceString(headers);
            var durationMillisString = durationMillis == long.MinValue ? string.Empty : $"DurationMillis={durationMillis}, ";

            logger.LogInformation(
                $"{messageType}, Verb={context.Request.Method}, Url={context.Request.GetDisplayUrl()}, Status={statusCode}, " +
                $"Ip={ipAddress}, {durationMillisString}Headers=[{headersString}], UserAgentInfo=[{GetUserAgentInfo(logger, context)}], Body={body}");
        }

        private static string GetHeaderTraceString(IHeaderDictionary headers)
        {
            return headers == null || !headers.Any()
                ? null
                : string.Join(",",
                    headers.Select(
                        header =>
                            string.Format("{0}='{1}'", header.Key,
                                string.Join(",", header.Value.Any() ? header.Value.ToString() : string.Empty)
                                .Limit(MaxHeaderLength))));
        }

        private static string GetUserAgentInfo(this ILogger logger, HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("User-Agent") ||
               context.Request.Headers["User-Agent"].ToString().IsNullOrWhiteSpace())
            {
                return "UserAgent is Undefined";
            }

            var clientInfo = Parser.GetDefault().Parse(context.Request.Headers["User-Agent"].ToString());

            return
                $"Browser={clientInfo.UserAgent.Family}.{clientInfo.UserAgent.Major}.{clientInfo.UserAgent.Minor}, " +
                $"OS={clientInfo.OS.Family}, " +
                $"Device={clientInfo.Device.Family.ValueOrUnknown()}.{clientInfo.Device.Brand.ValueOrUnknown()}.{clientInfo.Device.Model.ValueOrUnknown()}";
        }

        private static string ValueOrUnknown(this string value)
        {
            return value.IsNullOrWhiteSpace() ? "Unknown" : value;
        }
    }
}
