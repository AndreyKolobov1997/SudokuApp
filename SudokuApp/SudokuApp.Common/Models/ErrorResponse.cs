using System.Collections.Generic;
using SudokuApp.Common.Exceptions;

namespace SudokuApp.Common.Models
{
    public class ErrorResponse
    {
        public CustomErrorCode Code { get; set; }

        public string Message { get; set; }

        public string LogstashLink { get; set; }

        public IDictionary<string, object> InputErrors { get; set; }
    }
}