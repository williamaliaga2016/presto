using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

/// <summary>
/// Contrato de persistencia para el registro de confirmacion de soportes de pago.
/// </summary>
public interface ICargarSoportesPagoRepository
{
    /// <summary>
    /// Obtiene el registro activo mas reciente asociado al expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Soportes de Pago o `null` si no existe.</returns>
    Task<cargar_soportes_pago_entity?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Crea o actualiza el registro de confirmacion de soportes de pago del expediente.
    /// </summary>
    /// <param name="request">Entidad con datos de confirmacion y observaciones capturados en Cargar Soportes de Pago.</param>
    /// <param name="userId">Usuario autenticado que realiza la operacion.</param>
    /// <returns>Registro creado o actualizado.</returns>
    Task<cargar_soportes_pago_entity> Guardar(cargar_soportes_pago_entity request, int userId);
}
