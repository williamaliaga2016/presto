using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaLegalCierreCopiasPostventaRepository
        : IMultibancaGenericRepository<rectificatoria_legal_cierre_copias_postventa_entity>,
            IDisposable
    {
        Task<rectificatoria_legal_cierre_copias_postventa_entity?> GetByExpediente(long id_expediente);
    }
}
