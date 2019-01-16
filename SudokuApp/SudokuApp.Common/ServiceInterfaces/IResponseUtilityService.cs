using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SudokuApp.Common.Exceptions;

namespace SudokuApp.Common.ServiceInterfaces
{
    /// <summary>
    /// Сервис для подготовки единообразных HTTP-ответов.
    /// </summary>
    public interface IResponseUtilityService
    {
        /// <summary>
        /// Подготовка ответа, содержащего информацию об ошибках.
        /// </summary>
        /// <param name="context">HTTP-контекст.</param>
        /// <param name="code">HTTP-код для логирования.</param>
        /// <param name="inputErrors">Список ошибок.</param>
        /// <param name="customErrorCode">Переопределяемый HTTP-код для использования в ответе.</param>
        /// <param name="customMessageOverride">Переопределяемое сообщение об ошибке для использования в ответе.</param>
        /// <returns></returns>
        Task WriteErrorResponseAsync(
            HttpContext context, 
            HttpStatusCode code,
            IDictionary<string, object> inputErrors, 
            CustomErrorCode customErrorCode,
            string customMessageOverride);
    }
}