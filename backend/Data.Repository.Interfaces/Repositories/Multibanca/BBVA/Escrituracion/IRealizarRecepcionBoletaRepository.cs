using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarRecepcionBoletaRepository : IMultibancaGenericRepository<realizar_recepcion_boleta_entity>
{
    Task<realizar_recepcion_boleta_entity?> GetByExpediente(long idExpediente);
}
