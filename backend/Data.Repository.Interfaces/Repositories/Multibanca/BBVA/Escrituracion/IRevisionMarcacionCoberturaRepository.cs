using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRevisionMarcacionCoberturaRepository
    : IMultibancaGenericRepository<revision_marcacion_cobertura_entity>
{
    /// <summary>
    /// Retorna el registro activo (is_active=true, row_status=true) para el expediente.
    /// </summary>
    Task<revision_marcacion_cobertura_entity?> GetByExpediente(long idExpediente);
}
