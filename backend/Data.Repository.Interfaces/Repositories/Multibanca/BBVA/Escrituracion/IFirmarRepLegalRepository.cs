using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IFirmarRepLegalRepository : IMultibancaGenericRepository<firmar_rep_legal_entity>
{
    Task<firmar_rep_legal_entity?> GetByExpediente(long idExpediente);
}
