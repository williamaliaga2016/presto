using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface ICartaAprobacionBbvaRepository
{
    Task<carta_aprobacion_bbva?> GetByExpediente(long idExpediente);
    Task<List<carta_aprobacion_bbva>> GetHistoricoByExpediente(long idExpediente);
    Task<carta_aprobacion_bbva> Crear(carta_aprobacion_bbva entity);
    Task<carta_aprobacion_bbva> Actualizar(carta_aprobacion_bbva entity);
}
