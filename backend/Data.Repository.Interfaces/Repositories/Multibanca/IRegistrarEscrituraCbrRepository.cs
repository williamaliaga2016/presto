using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarEscrituraCbrRepository : IMultibancaGenericRepository<registrar_escritura_cbr_entity>, IDisposable
    {
        Task<registrar_escritura_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
