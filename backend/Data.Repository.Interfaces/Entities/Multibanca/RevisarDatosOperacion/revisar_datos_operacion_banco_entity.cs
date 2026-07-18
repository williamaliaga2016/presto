using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion
{
    public class revisar_datos_operacion_banco_entity
    {
        [Key]
        public int id_revisar_datos_operacion_banco { get; set; }
        public int id_revisar_datos_operacion { get; set; }
        public long id_expediente { get; set; }
        public bool? cuenta_carta_resguardo { get; set; }
        public string? institucion { get; set; }
        public string? rut_banco_acreedor { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
