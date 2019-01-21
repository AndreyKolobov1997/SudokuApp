using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Common.Exceptions;
using SudokuApp.DataAccess.Data.Entity;
using SudokuApp.DataAccess.RepositoryInterface;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace SudokuApp.DataAccess.Repository
{
    public class Repository<TContext> : IRepository, IDisposable where TContext : DbContext
    {
        private static readonly EntityState[] ConcurrencyStates =
        {
            EntityState.Added,
            EntityState.Modified
        };

        private readonly TContext _context;

        private bool _disposed;

        public Repository(
            TContext context)
        {
            _context = context;
        }

        public virtual void Add<TEntity>(
            TEntity entity)
            where TEntity : class, IBaseEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange<TEntity>(
            IList<TEntity> entities)
            where TEntity : class, IBaseEntity
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.ModifiedDate = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Added;
            }
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity
        {
            entity.ModifiedDate = DateTime.UtcNow;

            var trackedEntity = _context.ChangeTracker.Entries<TEntity>().SingleOrDefault(e => e.Entity.Id.Equals(entity.Id));

            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Modified;
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void UpdateRange<TEntity>(
            IList<TEntity> entities)
            where TEntity : class, IBaseEntity
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity
        {
            entity.DeletedDate = DateTime.UtcNow;

            Update(entity);
        }

        public virtual void DeleteRange<TEntity>(IList<TEntity> entities)
            where TEntity : class, IBaseEntity
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void DeleteById<TEntity>(object id)
            where TEntity : class, IBaseEntity
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }
        
        public virtual void DeleteHard<TEntity>(TEntity entity)
            where TEntity : class, IBaseEntity
        {
            var dbSet = _context.Set<TEntity>();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void DeleteRangeHard<TEntity>(IList<TEntity> entities)
            where TEntity : class, IBaseEntity
        {
            var dbSet = _context.Set<TEntity>();
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
            }
            dbSet.RemoveRange(entities);
        }

        public virtual void DeleteByIdHard<TEntity>(object id)
            where TEntity : class, IBaseEntity
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            DeleteHard(entity);
        }

        public virtual async Task Save()
        {
            var entities = _context.ChangeTracker.Entries().Where(x => ConcurrencyStates.Contains(x.State))
                .Select(x => x.Entity);

            var validationResults = new List<ValidationResult>();

            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    var fullErrorMessage = string.Join("; ", validationResults.Select(x => x.ErrorMessage));
                    throw new ValidationException(fullErrorMessage);
                }
            }

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var customConcurrencyException = new ConcurrencyException("Concurrency exception!", ex);
                foreach (var entry in ex.Entries)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    var concurrentEntityModel = new ConcurrencyExceptionEntity();
                    concurrentEntityModel.EntityType = entry.Entity.GetType();
                  
                    foreach (var property in proposedValues.Properties)
                    {
                        var concurrentValues = new ConcurrencyEntityValues(
                            property.Name,
                            proposedValues[property],
                            databaseValues[property]);

                        concurrentEntityModel.Values.Add(concurrentValues);
                    }

                    customConcurrencyException.Entities.Add(concurrentEntityModel);
                }

                throw customConcurrencyException;
            }

            DetachSavedEntities();
        }

        private void DetachSavedEntities()
        {
            var changedEntriesCopy = this._context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Unchanged).ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
