using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarDevolucionEPRepository : IMultibancaGenericRepository<realizar_devolucion_ep_entity>
{
    Task<realizar_devolucion_ep_entity?> GetByExpediente(long idExpediente);
}
