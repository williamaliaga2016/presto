using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IVoboGerenciaCohRepository
    : IMultibancaGenericRepository<vobo_gerencia_coh_entity>
{
    /// <summary>
    /// Retorna el registro activo (is_active=true, row_status=true) para el expediente.
    /// </summary>
    Task<vobo_gerencia_coh_entity?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Retorna los datos heredados de Excepcion Desembolso (y notaría de Firmar Escritura Cliente).
    /// Retorna null si no existe un registro activo de Excepcion Desembolso para el expediente.
    /// </summary>
    Task<VoboGerenciaCohHerencia?> GetDatosHerencia(long idExpediente);
}
