using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

/// <summary>
/// Contrato de aplicacion para el historico transversal de contactos BBVA.
/// </summary>
public interface IRegistroContactoApplication
{
    /// <summary>
    /// Consulta contactos por expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente.</param>
    /// <returns>Contactos ordenados de forma descendente.</returns>
    Task<List<registro_contacto_bbva>> GetByExpediente(long idExpediente);

    /// <summary>
    /// Consulta los catalogos propios de Registro Contacto.
    /// </summary>
    /// <returns>Catalogos de canal, resultado y detalle de contacto.</returns>
    Task<object> GetControles();

    /// <summary>
    /// Crea un contacto validando los campos obligatorios del modal.
    /// </summary>
    /// <param name="request">Datos capturados en Registro Contacto.</param>
    /// <param name="userId">Usuario autenticado que registra el contacto.</param>
    /// <returns>Contacto creado.</returns>
    Task<registro_contacto_bbva> Crear(registro_contacto_bbva request, int userId);
}
