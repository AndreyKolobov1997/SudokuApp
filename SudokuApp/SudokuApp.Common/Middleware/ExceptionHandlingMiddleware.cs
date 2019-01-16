using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SudokuApp.Common.Configuration.Options;
using SudokuApp.Common.Exceptions;
using SudokuApp.Common.Extensions;
using SudokuApp.Common.ServiceInterfaces;

namespace SudokuApp.Common.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ExceptionHandlingMiddlewareOptions _options;

        private readonly IResponseUtilityService _responseService;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IOptions<ExceptionHandlingMiddlewareOptions> options,
            IResponseUtilityService responseService)
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _options = options.Value;
            _responseService = responseService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadUserInputException ex)
            {
                var httpErrorCode = HttpStatusCode.BadRequest;
                var inputErrors = ex.InputErrors;
                var customErrorCode = ex.CustomErrorCode;
                var customOverrideMessage = ex.CustomOverrideMessage;

                if (ex is NotFoundException)
                {
                    httpErrorCode = HttpStatusCode.NotFound;
                }
                else if (ex is ConflictWithCurrentStateException)
                {
                    httpErrorCode = HttpStatusCode.Conflict;
                }

                await HandleErrorResponse(context, httpErrorCode, inputErrors, customErrorCode, customOverrideMessage).ConfigureAwait(false);
                _loggerFactory.CreateLogger(ex.Source).LogWarning(ex.ToString());
            }
            catch (Exception ex)
            {
                await HandleErrorResponse(
                    context, HttpStatusCode.InternalServerError, null, CustomErrorCode.InternalServerErrorDefault, (ex is InternalSystemException)
                    ? ((InternalSystemException)ex).CustomOverrideMessage
                    : string.Empty).ConfigureAwait(false);
                _loggerFactory.CreateLogger(ex.Source).LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Обрабатываем exception
        /// Если Request к /api, то возвращаем JSON
        /// Если Request к MVC, то редиректим на страницу, указанную в appsettings
        /// </summary>
        private async Task HandleErrorResponse(
            HttpContext context, HttpStatusCode code,
            IDictionary<string, object> inputErrors, CustomErrorCode customErrorCode,
            string customMessageOverride)
        {
            if (context.IsApiRequest() || context.Request.IsAjaxRequest())
            {
                await _responseService.WriteErrorResponseAsync(context, code, inputErrors, customErrorCode, customMessageOverride);
            }
            else
            {
                var redirectPath = context.Request.PathBase +
                                   _options.RedirectRelativePath +
                                   context.Request.QueryString;

                context.Response.Redirect(redirectPath);
            }
        }
    }
}
