namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class realizar_ep_registradas
{
    public long     id { get; set; }
    public long     id_expediente { get; set; }
    public string?  id_actividad { get; set; }

    // Campos de la actividad
    public DateTime? finalizacion { get; set; }
    public string?  causal { get; set; }
    public DateTime? fecha_finalizacion { get; set; }
    public bool     confirmacion_ep_registrada { get; set; }
    public string?  observaciones { get; set; }

    // Auditoría
    public bool      is_active { get; set; }
    public bool      row_status { get; set; }
    public int       created_by { get; set; }
    public DateTime  created_date { get; set; }
    public int?      modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
