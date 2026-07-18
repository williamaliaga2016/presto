using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

/// <summary>
/// Servicio de aplicacion para consultar, guardar y avanzar la actividad Validar Informacion.
/// </summary>
public interface IValidarInformacionApplication
{
    /// <summary>
    /// Consulta el formulario guardado para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente consultado.</param>
    /// <returns>Formulario guardado o null cuando aun no existe registro.</returns>
    Task<validar_informacion_bbva?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Guarda los datos del formulario de Validar Informacion.
    /// </summary>
    /// <param name="request">Datos capturados en la actividad.</param>
    /// <param name="userId">Usuario autenticado que guarda el formulario.</param>
    /// <returns>Formulario persistido.</returns>
    Task<validar_informacion_bbva> Guardar(validar_informacion_bbva request, int userId);

    Task<IReadOnlyList<titular_bbva>> GetTitulares(long idExpediente);

    Task<titular_bbva> AgregarTitular(long idExpediente, titular_bbva request, int userId);

    /// <summary>
    /// Avanza la actividad y retorna el resultado del workflow junto con el link temporal generado cuando aplica.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente que se avanza.</param>
    /// <param name="userId">Usuario autenticado que ejecuta el avance.</param>
    /// <param name="actividadId">Actividad funcional desde la cual se resuelve la transicion.</param>
    /// <returns>Resultado de workflow y acceso temporal opcional.</returns>
    Task<ValidarInformacionAvanzarResponseDTO> Avanzar(long idExpediente, int userId, string actividadId);

    /// <summary>
    /// Obtiene catalogos y controles requeridos por el formulario.
    /// </summary>
    /// <returns>Objeto con los catalogos usados por la pantalla.</returns>
    Task<object> GetControles();

    /// <summary>
    /// Consulta el formulario junto con la informacion de encabezado del expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente consultado.</param>
    /// <returns>Formulario y datos de encabezado.</returns>
    Task<object> GetFormularioConEncabezado(long idExpediente);
}
