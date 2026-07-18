using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IVisarOperacionRepository : IMultibancaGenericRepository<visar_operacion_entity>, IDisposable
    {
        Task<visar_operacion_entity?> GetByExpediente(long id_expediente);
    }
}
