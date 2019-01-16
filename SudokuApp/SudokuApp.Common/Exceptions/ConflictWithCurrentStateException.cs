using System;

namespace SudokuApp.Common.Exceptions
{
    public class ConflictWithCurrentStateException : BadUserInputException
    {
        public static readonly CustomErrorCode DefaultCustomErrorCode = CustomErrorCode.ItemAlreadyExistsDefault;

        public ConflictWithCurrentStateException(string message, Exception innerException) 
            : base(message, innerException, DefaultCustomErrorCode)
        {
        }

        public ConflictWithCurrentStateException(string message, Exception innerException, CustomErrorCode customErrorCode)
            : base(message, innerException, customErrorCode)
        {
        }

        public ConflictWithCurrentStateException(string message, CustomErrorCode customErrorCode) 
            : base(message, customErrorCode)
        {
        }

        public ConflictWithCurrentStateException(CustomErrorCode customErrorCode)
            : base(customErrorCode)
        {
        }

        public ConflictWithCurrentStateException(string message)
            : base(message, DefaultCustomErrorCode)
        {
        }

        public ConflictWithCurrentStateException()
        {
        }
    }
}