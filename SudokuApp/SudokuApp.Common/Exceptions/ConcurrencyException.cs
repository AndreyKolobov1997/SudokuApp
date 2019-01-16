using System;
using System.Collections.Generic;

namespace SudokuApp.Common.Exceptions
{
    public class ConcurrencyException : ConflictWithCurrentStateException
    {
        public ICollection<ConcurrencyExceptionEntity> Entities {get;set;}

        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
            Entities = new List<ConcurrencyExceptionEntity>();
        }
    }
}
