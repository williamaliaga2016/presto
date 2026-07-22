namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class firmar_rep_legal
{
    public long     id { get; set; }
    public long     id_expediente { get; set; }
    public string?  id_actividad { get; set; }

    // Campos de la actividad
    public string?  concepto_firma { get; set; }
    public string?  tipologia { get; set; }
    public string?  casuistica { get; set; }
    public string?  observaciones { get; set; }

    // Auditoría
    public bool      is_active { get; set; }
    public bool      row_status { get; set; }
    public int       created_by { get; set; }
    public DateTime  created_date { get; set; }
    public int?      modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
