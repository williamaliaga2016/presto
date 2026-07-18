using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class gestionar_firma_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "ACT_GESTIONAR_FIRMA";
    public bool? requiere_firma_electronica { get; set; }
    public bool firma_electronica_realizada { get; set; }
    public string? nombre_cliente_firma { get; set; }
    public string? nombre_solicitante_firma { get; set; }
    public TimeSpan? franja_horaria { get; set; }
    public string? direccion_firma { get; set; }
    public string? descripcion_tramite { get; set; }
    public DateTime? fecha_programacion { get; set; }
    public string? ciudad_cliente { get; set; }
    public string? tipo_credito_firma { get; set; }
    public string? observaciones { get; set; }
}
