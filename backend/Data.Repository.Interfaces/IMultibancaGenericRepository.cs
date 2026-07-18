using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IMultibancaGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity FindById(object id);
        IEnumerable<TEntity> All();
        void Insert(TEntity entity, int? userId);
        void Insert<T>(T entity, int? userId) where T : class;
        void Update(TEntity entity, int? userId);
        void Update<T>(T entity, int? userId) where T : class;
        void LogicalDeleteById(int id, int? userId);
        void LogicalDelete(TEntity entity, int? userId);
        void LogicalDelete<T>(T entity, int? userId) where T : class;
        void LogicalDeleteEntitiesById(List<int> entitiesId, int? userId);
        void LogicalDeleteEntities(List<TEntity> entities, int? userId);
        void PhysicalDeleteById(int id);
        void PhysicalDelete(TEntity entity);
        void PhysicalDelete<T>(T entity) where T : class;
        void PhysicalDeleteEntitiesById(List<int> entitiesId);
        void PhysicalDeleteEntities(List<TEntity> entities);

        #region METODOS SIN CLEAR (PARA ATOMICIDAD)
        void InsertWithoutClear(TEntity entity, int? userId);
        void InsertWithoutClear<T>(T entity, int? userId) where T : class;
        void UpdateWithoutClear(TEntity entity, int? userId);
        void UpdateWithoutClear<T>(T entity, int? userId) where T : class;
        #endregion
    }
}
