using System;

namespace SudokuApp.Common.Exceptions
{
    public class InternalSystemException : Exception
    {
        public string CustomOverrideMessage { get; set; }

        public InternalSystemException()
        {
        }

        public InternalSystemException(string message) : base(message)
        {
        }

        public InternalSystemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}