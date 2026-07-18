using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class calculo_generacion_documento_entity
    {
        [Key]
        public long id_calculo_generacion_documento { get; set; }
        public long id_expediente { get; set; }
        public string? revision_rol_propiedad { get; set; }
        public decimal? valor_uf_fecha_hoy { get; set; }
        public DateTime? fecha_calculo { get; set; }
        public decimal? valor_uf_fecha_calculo { get; set; }
        public bool is_enviar_reparo { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
