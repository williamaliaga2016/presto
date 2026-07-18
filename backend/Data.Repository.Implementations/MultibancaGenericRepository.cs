using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class MultibancaGenericRepository<TEntity> : IMultibancaGenericRepository<TEntity> where TEntity : class
    {
        private readonly MultibancaDBContext context;
        internal DbSet<TEntity> dbSet;

        public MultibancaGenericRepository(MultibancaDBContext _context)
        {
            context = _context;
            dbSet = context.Set<TEntity>();
        }

        public virtual TEntity FindById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return dbSet.ToList();
        }

        public virtual void Insert(TEntity entity, int? userId)
        {
            context.ChangeTracker.Clear();

            SetAuditInsertFields(entity, userId);
            NormalizeDateTimes(entity);

            dbSet.Add(entity);
        }

        public virtual void Insert<T>(T entity, int? userId) where T : class
        {
            context.ChangeTracker.Clear();

            SetAuditInsertFields(entity, userId);
            NormalizeDateTimes(entity);

            context.Set<T>().Add(entity);
        }

        public virtual void Update(TEntity entity, int? userId)
        {
            context.ChangeTracker.Clear();

            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update<T>(T entity, int? userId) where T : class
        {
            context.ChangeTracker.Clear();

            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void LogicalDelete(TEntity entity, int? userId)
        {
            context.ChangeTracker.Clear();

            SetLogicalDeleteFields(entity);
            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void LogicalDelete<T>(T entity, int? userId) where T : class
        {
            context.ChangeTracker.Clear();

            SetLogicalDeleteFields(entity);
            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void LogicalDeleteById(int id, int? userId)
        {
            context.ChangeTracker.Clear();

            TEntity entity = dbSet.Find(id);
            if (entity == null)
                return;

            LogicalDelete(entity, userId);
        }

        public virtual void LogicalDeleteEntities(List<TEntity> entities, int? userId)
        {
            foreach (TEntity entity in entities)
            {
                LogicalDelete(entity, userId);
            }
        }

        public virtual void LogicalDeleteEntitiesById(List<int> entitiesId, int? userId)
        {
            foreach (int id in entitiesId)
            {
                LogicalDeleteById(id, userId);
            }
        }

        public virtual void PhysicalDelete(TEntity entity)
        {
            context.ChangeTracker.Clear();

            dbSet.Attach(entity);
            dbSet.Remove(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void PhysicalDelete<T>(T entity) where T : class
        {
            context.ChangeTracker.Clear();

            context.Set<T>().Attach(entity);
            context.Set<T>().Remove(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void PhysicalDeleteById(int id)
        {
            context.ChangeTracker.Clear();

            var entity = dbSet.Find(id);
            if (entity == null)
                return;

            PhysicalDelete(entity);
        }

        public virtual void PhysicalDeleteEntities(List<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                PhysicalDelete(entity);
            }
        }

        public virtual void PhysicalDeleteEntitiesById(List<int> entitiesId)
        {
            foreach (int id in entitiesId)
            {
                PhysicalDeleteById(id);
            }
        }

        private void SetAuditInsertFields<T>(T entity, int? userId) where T : class
        {
            var entityType = typeof(T);

            var propertyCodeUser = entityType.GetProperty("created_by", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var propertyDate = entityType.GetProperty("created_date", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyCodeUser != null && propertyCodeUser.CanWrite)
            {
                SetPropertyValue(propertyCodeUser, entity, userId);
            }

            if (propertyDate != null && propertyDate.CanWrite)
            {
                var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                SetPropertyValue(propertyDate, entity, now);
            }
        }

        private void SetAuditUpdateFields<T>(T entity, int? userId) where T : class
        {
            var entityType = typeof(T);

            var propertyCodeUser = entityType.GetProperty("modified_by", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var propertyDate = entityType.GetProperty("modified_date", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyCodeUser != null && propertyCodeUser.CanWrite)
            {
                SetPropertyValue(propertyCodeUser, entity, userId);
            }

            if (propertyDate != null && propertyDate.CanWrite)
            {
                var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                SetPropertyValue(propertyDate, entity, now);
            }
        }

        private void SetLogicalDeleteFields<T>(T entity) where T : class
        {
            var entityType = typeof(T);

            var propertyIsActive = entityType.GetProperty("is_active", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var propertyRowStatus = entityType.GetProperty("row_status", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyIsActive != null && propertyIsActive.CanWrite)
            {
                SetPropertyValue(propertyIsActive, entity, false);
            }

            if (propertyRowStatus != null && propertyRowStatus.CanWrite)
            {
                SetPropertyValue(propertyRowStatus, entity, false);
            }
        }

        private void NormalizeDateTimes<T>(T entity) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                if (property.PropertyType == typeof(DateTime))
                {
                    var value = property.GetValue(entity);
                    if (value != null)
                    {
                        var dateValue = (DateTime)value;
                        var normalized = DateTime.SpecifyKind(dateValue, DateTimeKind.Unspecified);
                        property.SetValue(entity, normalized);
                    }
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    var value = (DateTime?)property.GetValue(entity);
                    if (value.HasValue)
                    {
                        var normalized = DateTime.SpecifyKind(value.Value, DateTimeKind.Unspecified);
                        property.SetValue(entity, normalized);
                    }
                }
            }
        }

        private void SetPropertyValue<T>(PropertyInfo property, T entity, object? value) where T : class
        {
            var propertyType = property.PropertyType;
            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (value == null)
            {
                if (Nullable.GetUnderlyingType(propertyType) != null || !propertyType.IsValueType)
                {
                    property.SetValue(entity, null);
                }
                return;
            }

            if (targetType == typeof(DateTime))
            {
                var dateValue = (DateTime)value;
                var normalized = DateTime.SpecifyKind(dateValue, DateTimeKind.Unspecified);
                property.SetValue(entity, normalized);
                return;
            }

            if (targetType == typeof(bool))
            {
                property.SetValue(entity, Convert.ToBoolean(value));
                return;
            }

            if (targetType == typeof(int))
            {
                property.SetValue(entity, Convert.ToInt32(value));
                return;
            }

            if (targetType == typeof(long))
            {
                property.SetValue(entity, Convert.ToInt64(value));
                return;
            }

            if (targetType == typeof(decimal))
            {
                property.SetValue(entity, Convert.ToDecimal(value));
                return;
            }

            if (targetType == typeof(string))
            {
                property.SetValue(entity, value.ToString());
                return;
            }

            property.SetValue(entity, Convert.ChangeType(value, targetType));
        }

        #region METODOS SIN CLEAR (PARA ATOMICIDAD)
        public virtual void InsertWithoutClear(TEntity entity, int? userId)
        {
            SetAuditInsertFields(entity, userId);
            NormalizeDateTimes(entity);

            dbSet.Add(entity);
        }

        public virtual void InsertWithoutClear<T>(T entity, int? userId) where T : class
        {
            SetAuditInsertFields(entity, userId);
            NormalizeDateTimes(entity);

            context.Set<T>().Add(entity);
        }

        public virtual void UpdateWithoutClear(TEntity entity, int? userId)
        {
            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateWithoutClear<T>(T entity, int? userId) where T : class
        {
            SetAuditUpdateFields(entity, userId);
            NormalizeDateTimes(entity);

            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
