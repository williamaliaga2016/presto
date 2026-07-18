using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReingresarEscrituraCbrRepository
        : IMultibancaGenericRepository<reingresar_escritura_cbr_entity>, IDisposable
    {
        Task<reingresar_escritura_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
