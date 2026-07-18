using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

public interface ICartaAprobacionBbvaApplication
{
    Task<carta_aprobacion_bbva?> GetByExpediente(long idExpediente);
    Task<List<carta_aprobacion_bbva>> GetHistorico(long idExpediente);
    Task<(bool success, string message)> Generar(long idExpediente, int idUsuario);
}
