using System;

namespace SudokuApp.DataAccess.Data.Entity
{
    public interface IBaseEntity
    {
        object Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        DateTime? DeletedDate { get; set; }
        Guid? CreatedBy { get; set; }
        Guid? ModifiedBy { get; set; }
        Guid? DeletedBy { get; set; }
        byte[] Version { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity
    {
        new T Id { get; set; }
    }
}
