using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaLegalFirmaAlzanteRepository
        : IMultibancaGenericRepository<rectificatoria_legal_firma_alzante_entity>,
            IDisposable
    {
        Task<rectificatoria_legal_firma_alzante_entity?> GetByExpediente(long id_expediente);
    }
}
