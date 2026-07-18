using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class carta_aprobacion_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_tipo_sub_producto { get; set; } = string.Empty;
    public int modelo_carta { get; set; }  // 1 o 2
    public string? nombre_archivo_docx { get; set; }
    public string? nombre_archivo_pdf { get; set; }
    public string? url_docx { get; set; }
    public string? url_pdf { get; set; }
    public string estado { get; set; } = "PENDIENTE"; // PENDIENTE | GENERADO | ERROR
    public string? error_detalle { get; set; }
    public int version { get; set; } = 1;
}
