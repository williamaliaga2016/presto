using AutoMapper;
using Common.Application.Interfaces;
using Data.Repository.Implementations;
using Data.Repository.Interfaces;

namespace Common.Application.Implementations
{
    public class MultibancaGenericApplication<TDomain, TEntity, TRepository> : IMultibancaGenericApplication<TDomain> where TDomain : class where TEntity : class where TRepository : IMultibancaGenericRepository<TEntity>
    {
        private readonly IMultibancaUnitOfWork _UoW;
        private readonly TRepository repositoryProvider;
        private readonly IMapper Mapper;
        private readonly MultibancaDBContext _dbContext;

        public MultibancaGenericApplication(MultibancaDBContext _multibancaDBContext, TRepository _repository, IMapper _mapper)
        {
            MultibancaGenericApplication<TDomain, TEntity, TRepository> genericApplication = this;
            _dbContext = _multibancaDBContext;
            _UoW = new MultibancaUnitOfWork(_dbContext);
            repositoryProvider = _repository;
            Mapper = _mapper;
        }

        private MultibancaUnitOfWork UnitOfWork
        {
            get
            {
                return _UoW as MultibancaUnitOfWork;
            }
        }

        public TRepository RepositoryProvider
        {
            get
            {
                return repositoryProvider;
            }
        }

        public IEnumerable<TDomain> All()
        {
            var list = RepositoryProvider.All();
            return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDomain>>(list);
        }

        public TDomain Create(TDomain model, int? userId)
        {
            TEntity entity = Mapper.Map<TDomain, TEntity>(model);
            RepositoryProvider.Insert(entity, userId);
            UnitOfWork.Save();
            return Mapper.Map<TEntity, TDomain>(entity);
        }

        public TDomain FindId(int id)
        {
            var model = RepositoryProvider.FindById(id);
            return Mapper.Map<TEntity, TDomain>(model);
        }

        public TDomain FindId(long id)
        {
            var model = RepositoryProvider.FindById(id);
            return Mapper.Map<TEntity, TDomain>(model);
        }

        public void LogicalDelete(TDomain model, int? userId)
        {
            RepositoryProvider.LogicalDelete(Mapper.Map<TDomain, TEntity>(model), userId);
            UnitOfWork.Save();
        }

        public void LogicalDeleteById(int entityIds, int? userId)
        {
            RepositoryProvider.LogicalDeleteById(entityIds, userId);
            UnitOfWork.Save();
        }

        public void LogicalDeleteEntities(List<TDomain> entities, int? userId)
        {
            RepositoryProvider.LogicalDeleteEntities(Mapper.Map<List<TDomain>, List<TEntity>>(entities), userId);
            UnitOfWork.Save();
        }

        public void LogicalDeleteEntitiesById(List<int> entitieIds, int? userId)
        {
            RepositoryProvider.LogicalDeleteEntitiesById(entitieIds, userId);
            UnitOfWork.Save();
        }

        public TDomain Update(TDomain model, int? userId)
        {
            TEntity entity = Mapper.Map<TDomain, TEntity>(model);
            RepositoryProvider.Update(entity, userId);
            UnitOfWork.Save();
            return Mapper.Map<TEntity, TDomain>(entity);
        }

        // Metodos nuevos para crear registros sin afectar la atomicidad
        public TDomain CreateWithoutSaveChanges(TDomain model, int? userId)
        {
            TEntity entity = Mapper.Map<TDomain, TEntity>(model);
            RepositoryProvider.InsertWithoutClear(entity, userId);
            return Mapper.Map<TEntity, TDomain>(entity);
        }

        // Metodos nuevos para crear registros sin afectar la atomicidad
        public TDomain UpdateWithoutSaveChanges(TDomain model, int? userId)
        {
            TEntity entity = Mapper.Map<TDomain, TEntity>(model);
            RepositoryProvider.UpdateWithoutClear(entity, userId);
            return Mapper.Map<TEntity, TDomain>(entity);
        }
    }
}
