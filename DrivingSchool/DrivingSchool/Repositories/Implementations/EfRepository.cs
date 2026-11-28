using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DrivingSchool.Repositories.Implementations
{
    public abstract class EfRepository<TEntity> where TEntity : class
    {
        private readonly Func<Owned<DataContext>> _openContext;
        private readonly string _keyName;
        private readonly Func<TEntity, int> _getId;
        protected Serilog.ILogger Logger { get; }

        protected EfRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger,
            string keyName, Func<TEntity, int> getId)
        {
            _openContext = openContext ?? throw new ArgumentNullException(nameof(openContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _keyName = keyName;
            _getId = getId;
        }

        protected TResult Run<TResult>(string operation, int? id, Func<DataContext, TResult> action)
        {
            Logger.Debug("{Operation} started for entity ID {EntityId}", operation, id);
            try
            {
                using var scope = _openContext();
                return action(scope.Value);
            }
            catch (Exception ex)
            {
                var sqlState = (ex as PostgresException ?? ex.InnerException as PostgresException)?.SqlState;
                Logger.Error("{Operation} failed for entity ID {EntityId}. Error type: {ErrorType}; SQL state: {SqlState}",
                    operation, id, ex.GetType().Name, sqlState);
                throw;
            }
        }

        protected IQueryable<TEntity> OrderById(IQueryable<TEntity> query) =>
            query.OrderBy(entity => EF.Property<int>(entity, _keyName));

        protected IReadOnlyList<TEntity> ReadAll(string operation) => Run(operation, null, context =>
        {
            var result = OrderById(context.Set<TEntity>().AsNoTracking()).ToList();
            Logger.Information("{Operation} returned {Count} records", operation, result.Count);
            return result;
        });

        protected TEntity ReadById(string operation, int id) => Run(operation, id, context =>
        {
            var entity = context.Set<TEntity>().AsNoTracking()
                .SingleOrDefault(item => EF.Property<int>(item, _keyName) == id);
            Logger.Information("{Operation} completed for entity ID {EntityId}. Found: {Found}",
                operation, id, entity != null);
            return entity;
        });

        protected void Create(string operation, TEntity entity) =>
            Run(operation, entity == null ? null : _getId(entity), context =>
            {
                ArgumentNullException.ThrowIfNull(entity);
                context.Set<TEntity>().Add(entity);
                var count = context.SaveChanges();
                LogSaved(operation, _getId(entity), count, "Created");
                return count;
            });

        protected void Update(string operation, TEntity entity, Action<DataContext, TEntity, TEntity> copy) =>
            Run(operation, entity == null ? null : _getId(entity), context =>
            {
                ArgumentNullException.ThrowIfNull(entity);
                var id = _getId(entity);
                var existing = FindExisting(context, id);
                copy(context, existing, entity);
                var count = context.SaveChanges();
                LogSaved(operation, id, count, count == 0 ? "Unchanged" : "Updated");
                return count;
            });

        protected void Delete(string operation, int id) => Run(operation, id, context =>
        {
            context.Set<TEntity>().Remove(FindExisting(context, id));
            var count = context.SaveChanges();
            LogSaved(operation, id, count, "Deleted");
            return count;
        });

        protected void LogSaved(string operation, int id, int count, string result) =>
            Logger.Information("{Operation} completed for entity ID {EntityId}: {Result}; saved {Count} entries",
                operation, id, result, count);

        private static TEntity FindExisting(DataContext context, int id) => context.Set<TEntity>().Find(id)
            ?? throw new InvalidOperationException("Запись больше не существует.");
    }
}
