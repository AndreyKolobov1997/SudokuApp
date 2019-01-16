using System;
using System.Collections.Generic;
using System.Net;
using SudokuApp.Common.Extensions;

namespace SudokuApp.Common.Exceptions
{
    public class BadUserInputException : Exception
    {
        public CustomErrorCode CustomErrorCode = CustomErrorCode.BadUserInputDefault;
        public readonly IDictionary<string, object> InputErrors;

        public string CustomOverrideMessage { get; set; }
        
        public HttpStatusCode StatusCode { get; set; }

        public BadUserInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadUserInputException(string message, Exception innerException, CustomErrorCode customErrorCode) : base(
            message, innerException)
        {
            CustomErrorCode = customErrorCode;
        }

        public BadUserInputException(string message) : base(message)
        {
        }

        public BadUserInputException(string message, CustomErrorCode customErrorCode) : base(message)
        {
            CustomErrorCode = customErrorCode;
        }

        public BadUserInputException(CustomErrorCode customErrorCode) : base(customErrorCode.GetEnumDescription())
        {
            CustomErrorCode = customErrorCode;
        }

        public BadUserInputException(IDictionary<string, object> inputErrors) : base(CustomErrorCode
            .BadUserInputDefault.GetEnumDescription())
        {
            InputErrors = inputErrors;
        }

        public BadUserInputException()
        {
        }
    }
}