using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

/// <summary>
/// Servicio de aplicacion para la actividad Cargar Documentos Cliente.
/// </summary>
public interface ICargarDocumentosClienteApplication
{
    /// <summary>
    /// Obtiene el registro activo de confirmacion documental para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Documentos Cliente o `null` si no existe.</returns>
    Task<cargar_documentos_cliente?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Guarda los datos de confirmacion sin avanzar la actividad de workflow.
    /// </summary>
    /// <param name="request">Datos capturados en la actividad Cargar Documentos Cliente.</param>
    /// <param name="userId">Usuario autenticado que guarda el registro.</param>
    /// <returns>Registro creado o actualizado.</returns>
    Task<cargar_documentos_cliente> Guardar(cargar_documentos_cliente request, int userId);

    /// <summary>
    /// Obtiene informacion general del cliente y analista para mostrarla en forma segura al usuario externo.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Informacion general para la pantalla de Cargar Documentos Cliente.</returns>
    Task<CargarDocumentosClienteInfoDTO> GetInfoCliente(long idExpediente);

    /// <summary>
    /// Valida la confirmacion documental y avanza la actividad hacia revision documental.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <param name="userId">Usuario autenticado que ejecuta el avance.</param>
    /// <param name="actividadId">Identificador de actividad workflow actual.</param>
    /// <returns>Actividades asignadas por el motor de workflow despues del avance.</returns>
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId, string actividadId);
}
