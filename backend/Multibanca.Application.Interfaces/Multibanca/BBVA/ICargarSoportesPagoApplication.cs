using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

/// <summary>
/// Servicio de aplicacion para la actividad Cargar Soportes de Pago.
/// </summary>
public interface ICargarSoportesPagoApplication
{
    /// <summary>
    /// Obtiene el registro activo de confirmacion de soportes de pago para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Soportes de Pago o `null` si no existe.</returns>
    Task<cargar_soportes_pago?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Guarda los datos de confirmacion sin avanzar la actividad de workflow.
    /// </summary>
    /// <param name="request">Datos capturados en la actividad Cargar Soportes de Pago.</param>
    /// <param name="userId">Usuario autenticado que guarda el registro.</param>
    /// <returns>Registro creado o actualizado.</returns>
    Task<cargar_soportes_pago> Guardar(cargar_soportes_pago request, int userId);

    /// <summary>
    /// Obtiene informacion general del cliente y analista para mostrarla al usuario externo.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Informacion general para la pantalla de Cargar Soportes de Pago.</returns>
    Task<CargarSoportesPagoInfoDTO> GetInfoCliente(long idExpediente);

    /// <summary>
    /// Valida la confirmacion de soportes de pago y avanza la actividad.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <param name="userId">Usuario autenticado que ejecuta el avance.</param>
    /// <param name="actividadId">Identificador de actividad workflow actual.</param>
    /// <returns>Actividades asignadas por el motor de workflow despues del avance.</returns>
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId, string actividadId);
}
