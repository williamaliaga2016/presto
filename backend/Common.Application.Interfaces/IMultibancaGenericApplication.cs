namespace Common.Application.Interfaces
{
    public interface IMultibancaGenericApplication<TDomain> where TDomain : class
    {
        TDomain FindId(int id);
        TDomain FindId(long id);
        IEnumerable<TDomain> All();
        TDomain Create(TDomain model, int? userId);
        TDomain Update(TDomain model, int? userId);
        void LogicalDelete(TDomain model, int? userId);
        void LogicalDeleteById(int entityIds, int? userId);
        void LogicalDeleteEntities(List<TDomain> entities, int? userId);
        void LogicalDeleteEntitiesById(List<int> entitieIds, int? userId);
        TDomain CreateWithoutSaveChanges(TDomain model, int? userId);
        TDomain UpdateWithoutSaveChanges(TDomain model, int? userId);
    }
}
