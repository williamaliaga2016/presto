namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class batch_log_bbva
{
    public long id { get; set; }
    public string nombre_archivo { get; set; } = string.Empty;
    public string? ruta_archivo { get; set; }
    public int total_filas { get; set; }
    public int filas_exitosas { get; set; }
    public int filas_error { get; set; }
    public string estado { get; set; } = "EN_PROCESO";
    public string? detalle_errores { get; set; }  // JSON serializado
    public DateTime fecha_inicio { get; set; } = DateTime.Now;
    public DateTime? fecha_fin { get; set; }
    public int? ejecutado_por { get; set; }
}
