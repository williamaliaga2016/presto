using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class registro_contacto_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = string.Empty;
    public int id_usuario { get; set; }
    public string canal_contacto { get; set; } = string.Empty;
    public string resultado_contacto { get; set; } = string.Empty;
    public int? nro_contacto { get; set; }
    public string? detalle_contacto { get; set; }
    public bool? inmueble_definido { get; set; }
    public string? observaciones { get; set; }
    public DateTime fecha_contacto { get; set; } = DateTime.Now;
}
