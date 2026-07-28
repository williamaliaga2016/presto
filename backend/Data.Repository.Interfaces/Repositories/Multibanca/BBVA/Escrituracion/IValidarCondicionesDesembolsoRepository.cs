using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IValidarCondicionesDesembolsoRepository : IMultibancaGenericRepository<validar_condiciones_desembolso_entity>
{
    Task<validar_condiciones_desembolso_entity?> GetByExpediente(long idExpediente);
}
