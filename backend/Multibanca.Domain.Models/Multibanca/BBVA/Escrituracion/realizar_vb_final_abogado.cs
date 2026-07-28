namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class realizar_vb_final_abogado
{
    public long     id { get; set; }
    public long     id_expediente { get; set; }
    public string?  id_actividad { get; set; }

    public string?  requiere_devolucion { get; set; }
    public string?  tipologia { get; set; }
    public string?  casuistica { get; set; }
    public string?  origen_tramite { get; set; }
    public bool     bandera_excepcion { get; set; }
    public string?  observaciones { get; set; }

    public bool      is_active { get; set; }
    public bool      row_status { get; set; }
    public int       created_by { get; set; }
    public DateTime  created_date { get; set; }
    public int?      modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
