using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarEntregaEpFirmadaRepository : IMultibancaGenericRepository<realizar_entrega_ep_firmada_entity>
{
    Task<realizar_entrega_ep_firmada_entity?> GetByExpediente(long idExpediente);
}
