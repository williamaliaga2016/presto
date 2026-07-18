namespace Multibanca.DTO.Multibanca.BBVA;

/// <summary>
/// Contrato de informacion general enmascarable para la pantalla externa de Cargar Documentos Cliente.
/// </summary>
public class CargarDocumentosClienteInfoDTO
{
    /// <summary>
    /// Identificador del expediente que se presenta como folio Presto.
    /// </summary>
    public long id_expediente { get; set; }

    /// <summary>
    /// Nombre del cliente obtenido desde la informacion validada.
    /// </summary>
    public string? nombre_completo_t1 { get; set; }

    /// <summary>
    /// Alias conservado para consumidores existentes que leen el nombre del cliente.
    /// </summary>
    public string? nombre_cliente { get; set; }

    /// <summary>
    /// Correo declarativo conservado por compatibilidad del contrato.
    /// </summary>
    public string? correo_declarativo { get; set; }

    /// <summary>
    /// Telefono declarativo conservado por compatibilidad del contrato.
    /// </summary>
    public string? telefono_declarativo { get; set; }

    /// <summary>
    /// Usuario responsable del workflow presentado como analista de vivienda.
    /// </summary>
    public string? nombre_analista { get; set; }
}
