using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.DataAccess.RepositoryInterface
{
    public interface IRepository
    {
        void Add<TEntity>(
            TEntity entity)
            where TEntity : class, IBaseEntity;

        void AddRange<TEntity>(
            IList<TEntity> entities)
            where TEntity : class, IBaseEntity;

        void Update<TEntity>(
            TEntity entity)
            where TEntity : class, IBaseEntity;

        void UpdateRange<TEntity>(
            IList<TEntity> entities)
            where TEntity : class, IBaseEntity;

        void Delete<TEntity>(
            TEntity entity)
            where TEntity : class, IBaseEntity;

        void DeleteRange<TEntity>(
            IList<TEntity> entities)
            where TEntity : class, IBaseEntity;

        void DeleteById<TEntity>(
            object id)
            where TEntity : class, IBaseEntity;

        void DeleteHard<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity;

        void DeleteRangeHard<TEntity>(IList<TEntity> entities)
            where TEntity : class, IBaseEntity;

        void DeleteByIdHard<TEntity>(object id)
            where TEntity : class, IBaseEntity;

        Task Save();
    }
}
