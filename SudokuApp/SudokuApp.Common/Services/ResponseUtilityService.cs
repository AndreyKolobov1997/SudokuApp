using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SudokuApp.Common.Configuration.Options;
using SudokuApp.Common.Exceptions;
using SudokuApp.Common.Extensions;
using SudokuApp.Common.Models;
using SudokuApp.Common.ServiceInterfaces;

namespace SudokuApp.Common.Services
{
    /// <inheritdoc />
    public class ResponseUtilityService : IResponseUtilityService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHostingEnvironment _env;
        private readonly ExceptionHandlingMiddlewareOptions _options;

        public ResponseUtilityService(
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IOptions<ExceptionHandlingMiddlewareOptions> options)
        {
            _loggerFactory = loggerFactory;
            _env = env;
            _options = options.Value;
        }

        public async Task WriteErrorResponseAsync(
            HttpContext context,
            HttpStatusCode code,
            IDictionary<string, object> inputErrors,
            CustomErrorCode customErrorCode,
            string customMessageOverride)
        {
            var response = context.Response;
            response.ContentType = "application/json; charset=utf8";
            response.StatusCode = (int)code;

            await response
                .WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = new ErrorResponse
                    {
                        Message = string.IsNullOrWhiteSpace(customMessageOverride) ? customErrorCode.GetEnumDescription() : customMessageOverride,
                        LogstashLink = _env.IsDevelopment() ? "Логи не доступны для среды разработки" : GetLogstashLink(context),
                        InputErrors = inputErrors,
                        Code = customErrorCode,
                    },
                }))
                .ConfigureAwait(false);
        }

        #region Private

        private string GetLogstashLink(HttpContext context)
        {
            var traceId = context?.TraceIdentifier;

            if (string.IsNullOrEmpty(traceId))
            {
                return string.Empty;
            }

            // add logstash search url to response
            var currentTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var endTimeInSec = (int)currentTime.TotalSeconds + 5; // Add a little padding time, just in case
            var startTimeInSec = endTimeInSec - 600; // Assume the request took less than 10 minutes

            if (!_env.IsDevelopment())
            {
                _loggerFactory.CreateLogger<ResponseUtilityService>()
                    .LogError($"Invalid settings provided for Logstash link generation.");
            }

            return string.Format(_options.LogstashUrlTemplate, startTimeInSec, endTimeInSec, traceId);
        }

        #endregion
    }
}