using System;

namespace SudokuApp.Common.Exceptions
{
    public class NotFoundException : BadUserInputException
    {
        public static readonly CustomErrorCode DefaultCustomErrorCode = CustomErrorCode.NotFoundDefault;

        public NotFoundException(string message, Exception innerException) 
            : base(message, innerException, DefaultCustomErrorCode)
        {
        }

        public NotFoundException(string message, Exception innerException, CustomErrorCode customErrorCode)
            : base(message, innerException, customErrorCode)
        {
        }

        public NotFoundException(string message, CustomErrorCode customErrorCode
            ) : base(message, customErrorCode)
        {
        }

        public NotFoundException(CustomErrorCode customErrorCode
        ) : base(customErrorCode)
        {
        }

        public NotFoundException(string message) : base(message, DefaultCustomErrorCode)
        {
        }

        public NotFoundException()
        {
        }
    }
}