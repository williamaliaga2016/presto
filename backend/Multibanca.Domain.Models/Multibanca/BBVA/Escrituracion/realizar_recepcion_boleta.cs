namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class realizar_recepcion_boleta
{
    public long     id { get; set; }
    public long     id_expediente { get; set; }
    public string?  id_actividad { get; set; }

    // Campos VUR
    public string?  numero_boleta { get; set; }
    public DateTime? fecha_boleta { get; set; }
    public string?  numero_matricula { get; set; }

    // Campos transaccionales
    public string?  tipo_boleta { get; set; }
    public string?  boleta_en_poder_de { get; set; }
    public string?  codigo_zona { get; set; }
    public string?  oficina_registro { get; set; }
    public bool     boleta_recibida { get; set; }
    public string?  aplica_excepcion { get; set; }

    // Control VUR
    public bool     vur_ejecutado { get; set; }
    public bool     vur_exitoso { get; set; }
    public int      vur_intentos { get; set; }

    public string?  observaciones { get; set; }

    // Auditoría
    public bool      is_active { get; set; }
    public bool      row_status { get; set; }
    public int       created_by { get; set; }
    public DateTime  created_date { get; set; }
    public int?      modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
