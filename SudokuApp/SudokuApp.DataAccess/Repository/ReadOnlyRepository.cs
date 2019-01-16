using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuApp.DataAccess.Data.Entity;
using SudokuApp.DataAccess.RepositoryInterface;

namespace SudokuApp.DataAccess.Repository
{
    public class ReadOnlyRepository<TContext> : IReadOnlyRepository where TContext : DbContext
    {
        protected readonly TContext _context;

        public ReadOnlyRepository(TContext context)
        {
            _context = context;
        }

        protected virtual IQueryable<TEntity> PrepareQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            includeProperties = includeProperties ?? string.Empty; //инклюдит навигационные свойства
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter); //применяет фильтр
            }

            if (!includeDeleted) //смотрит надо ли добавлять в выборку удалённые
            {
                query = query.Where(x => !x.DeletedDate.HasValue);
            }


            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, //инклюдит навигационные свойства
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) //применяет сортировку если она есть
            {
                query = orderBy(query);
            }

            if (skip.HasValue) //применяет пропуски если они есть
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue) //отсекает заданное количество
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return PrepareQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).AsNoTracking(); //получает результат от PrepareQueryable и отключает их отслеживание с помощью AsNoTracking
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll<TEntity>(
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return await GetQueryable(null, includeDeleted, orderBy, includeProperties, skip, take).ToListAsync()
                .ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<TEntity>> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).ToListAsync()
                .ConfigureAwait(false);
        }

        public virtual Task<TEntity> GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            string includeProperties = null)
            where TEntity : class, IBaseEntity
        {
            return GetQueryable(filter, includeDeleted, null, includeProperties).SingleOrDefaultAsync();
        }

        public virtual Task<TEntity> GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class, IBaseEntity
        {
            return GetQueryable(filter, includeDeleted, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public virtual Task<int> GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false)
            where TEntity : class, IBaseEntity
        {
            return GetQueryable(filter, includeDeleted).CountAsync();
        }

        public virtual Task<bool> CheckIfExist<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false)
            where TEntity : class, IBaseEntity
        {
            return GetQueryable(filter, includeDeleted).AnyAsync();
        }

        public virtual async Task<IEnumerable<TSelect>> GetSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).Select(select)
                .ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<TSelect>> GetSelectable<TEntity, TSelect>(
            string selectProperty,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            var parameter = Expression.Parameter(typeof(TEntity), "parameter");
            var body = Expression.PropertyOrField(parameter, selectProperty);
            var lambdaSelect = Expression.Lambda<Func<TEntity, TSelect>>(body, parameter);

            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).Select(lambdaSelect)
                .ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<TSelect> GetOneSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).Select(select)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<TSelect> GetOneSelectable<TEntity, TSelect>(
            string selectProperty,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            var parameter = Expression.Parameter(typeof(TEntity), "parameter");
            var body = Expression.PropertyOrField(parameter, selectProperty);
            var lambdaSelect = Expression.Lambda<Func<TEntity, TSelect>>(body, parameter);

            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).Select(lambdaSelect)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<TSelect> GetFirstSelectable<TEntity, TSelect>(
            Expression<Func<TEntity, TSelect>> select,
            Expression<Func<TEntity, bool>> filter = null,
            bool includeDeleted = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class, IBaseEntity
        {
            return await GetQueryable(filter, includeDeleted, orderBy, includeProperties, skip, take).Select(select)
                .FirstOrDefaultAsync();
        }
    }
}