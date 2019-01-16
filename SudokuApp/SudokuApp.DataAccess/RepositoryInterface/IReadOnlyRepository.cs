using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.DataAccess.RepositoryInterface
{
    public interface IReadOnlyRepository
    {
        Task<IEnumerable<TEntity>> GetAll<TEntity>(
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<IEnumerable<TEntity>> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<TEntity> GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            string includeProperties = null)
            where TEntity : class, IBaseEntity;

        Task<TEntity> GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class, IBaseEntity;

        Task<int> GetCount<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false)
            where TEntity : class, IBaseEntity;

        Task<bool> CheckIfExist<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            bool includeDeleted = false)
            where TEntity : class, IBaseEntity;

        Task<IEnumerable<TSelect>> GetSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<IEnumerable<TSelect>> GetSelectable<TEntity, TSelect>(
            string selectProperty,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<TSelect> GetOneSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<TSelect> GetOneSelectable<TEntity, TSelect>(
            string selectProperty,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;

        Task<TSelect> GetFirstSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity;
    }
}
