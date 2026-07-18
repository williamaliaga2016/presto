using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

/// <summary>
/// Datos propios de la actividad BBV-44 Definir Inmueble.
/// </summary>
public class definir_inmueble_bbva : base_auditoria
{
    /// <summary>
    /// Identificador tecnico del registro.
    /// </summary>
    public long id { get; set; }

    /// <summary>
    /// Identificador del expediente asociado.
    /// </summary>
    public long id_expediente { get; set; }

    /// <summary>
    /// Actividad de workflow que origina el registro.
    /// </summary>
    public string id_actividad { get; set; } = "BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803";

    /// <summary>
    /// Indica si el cliente ya cuenta con inmueble definido.
    /// </summary>
    public bool? cliente_cuenta_inmueble_definido { get; set; }

    /// <summary>
    /// Nombre de la constructora capturado como texto libre.
    /// </summary>
    public string? constructora { get; set; }

    /// <summary>
    /// Fecha estimada de entrega del inmueble.
    /// </summary>
    public DateTime? fecha_estimada_entrega { get; set; }

    /// <summary>
    /// Estatus general elegido para el avance de la actividad.
    /// </summary>
    public string? estatus_general { get; set; } = "SIN_INM";

    /// <summary>
    /// Motivo asociado al estatus cuando aplica.
    /// </summary>
    public string? motivo_devolucion { get; set; }

    /// <summary>
    /// Observaciones capturadas por el analista.
    /// </summary>
    public string? observaciones { get; set; }
}
