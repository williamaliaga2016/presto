using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICargarDocumentosConstructoraRepository : IMultibancaGenericRepository<cargar_documentos_constructora_entity>, IDisposable
    {
        Task<cargar_documentos_constructora_entity?> GetByExpediente(long idExpediente);
    }
}
