using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

/// <summary>
/// Contrato de persistencia para el historico transversal de contactos BBVA.
/// </summary>
public interface IRegistroContactoRepository
{
    /// <summary>
    /// Consulta los contactos activos de un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente.</param>
    /// <returns>Lista de contactos en orden descendente.</returns>
    Task<List<registro_contacto_bbva>> GetByExpediente(long idExpediente);

    /// <summary>
    /// Crea un nuevo contacto asignando numero secuencial por expediente.
    /// </summary>
    /// <param name="request">Datos del contacto capturados por la pantalla.</param>
    /// <param name="userId">Usuario autenticado que registra el contacto.</param>
    /// <returns>Registro persistido.</returns>
    Task<registro_contacto_bbva> Crear(registro_contacto_bbva request, int userId);
}
