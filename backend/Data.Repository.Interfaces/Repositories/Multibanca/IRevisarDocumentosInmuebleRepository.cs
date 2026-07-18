using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarDocumentosInmuebleRepository
        : IMultibancaGenericRepository<revisar_documentos_inmueble_entity>, IDisposable
    {
        Task<revisar_documentos_inmueble_entity?> GetByExpediente(long idExpediente);
    }
}
