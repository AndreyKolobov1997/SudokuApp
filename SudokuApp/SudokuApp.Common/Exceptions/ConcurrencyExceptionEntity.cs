using System;
using System.Collections.Generic;

namespace SudokuApp.Common.Exceptions
{
    public class ConcurrencyExceptionEntity
    {
        public Type EntityType { get; set; }

        public ICollection<ConcurrencyEntityValues> Values { get; set; }

        public ConcurrencyExceptionEntity()
        {
            Values = new List<ConcurrencyEntityValues>();
        }
    }

    public class ConcurrencyEntityValues
    {
        public string PropertyName { get; set; }

        public object ProposedValue { get; set; }

        public object DataBaseValue { get; set; }

        public ConcurrencyEntityValues(string propertyName, object proposedValue, object dataBaseValue)
        {
            PropertyName = propertyName;
            ProposedValue = proposedValue;
            DataBaseValue = dataBaseValue;
        }
    }
}
