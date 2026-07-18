using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

/// <summary>
/// Contrato de persistencia para el registro de confirmacion documental de Cargar Documentos Cliente.
/// </summary>
public interface ICargarDocumentosClienteRepository
{
    /// <summary>
    /// Obtiene el registro activo mas reciente asociado al expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Documentos Cliente o `null` si no existe.</returns>
    Task<cargar_documentos_cliente_entity?> GetByExpediente(long idExpediente);

    /// <summary>
    /// Crea o actualiza el registro de confirmacion documental del expediente.
    /// </summary>
    /// <param name="request">Entidad con datos de confirmacion y observaciones capturados en Cargar Documentos Cliente.</param>
    /// <param name="userId">Usuario autenticado que realiza la operacion.</param>
    /// <returns>Registro creado o actualizado.</returns>
    Task<cargar_documentos_cliente_entity> Guardar(cargar_documentos_cliente_entity request, int userId);
}
