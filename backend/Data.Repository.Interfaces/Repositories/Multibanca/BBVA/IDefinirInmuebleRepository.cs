using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

/// <summary>
/// Contrato de persistencia de la actividad BBV-44 Definir Inmueble.
/// </summary>
public interface IDefinirInmuebleRepository
{
    /// <summary>
    /// Obtiene el ultimo registro activo de Definir Inmueble por expediente.
    /// </summary>
    Task<definir_inmueble_bbva?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Inserta o actualiza los datos propios de Definir Inmueble.
    /// </summary>
    Task<definir_inmueble_bbva> Guardar(definir_inmueble_bbva request, int userId);
}
