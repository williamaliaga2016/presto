using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaLegalCartaResguardoRepository
        : IMultibancaGenericRepository<rectificatoria_legal_carta_resguardo_entity>,
            IDisposable
    {
        Task<rectificatoria_legal_carta_resguardo_entity?> GetByExpediente(long id_expediente);
    }
}
