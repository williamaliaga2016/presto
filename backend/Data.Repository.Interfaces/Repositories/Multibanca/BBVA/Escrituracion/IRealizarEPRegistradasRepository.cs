using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarEPRegistradasRepository : IMultibancaGenericRepository<realizar_ep_registradas_entity>
{
    Task<realizar_ep_registradas_entity?> GetByExpediente(long idExpediente);
}
