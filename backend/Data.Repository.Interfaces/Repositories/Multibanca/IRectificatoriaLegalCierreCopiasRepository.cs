using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaLegalCierreCopiasRepository
        : IMultibancaGenericRepository<rectificatoria_legal_cierre_copias_entity>,
            IDisposable
    {
        Task<rectificatoria_legal_cierre_copias_entity?> GetByExpediente(long id_expediente);
    }
}
