using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IExcepcionDesembolsoRepository : IMultibancaGenericRepository<excepcion_desembolso_entity>
{
    Task<excepcion_desembolso_entity?> GetByExpediente(long idExpediente);
    Task<ExcepcionDesembolsoHerencia?> GetDatosHerencia(long idExpediente);
    Task<OrigenCasoEnum?> DeterminarOrigenCaso(long idExpediente);
}
