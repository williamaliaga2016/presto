using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class tasacion_detalle_entity
    {
        [Key]
        public int id_tasacion_detalle { get; set; }
        public int id_tasacion { get; set; }
        public long id_expediente { get; set; }
        public string? tipo_tasacion { get; set; }
        public string? nro_tasacion_p1 { get; set; }
        public string? nro_tasacion_p2 { get; set; }
        public string? nro_tasacion_p3 { get; set; }
        public string? superficie_edificada { get; set; }
        public string? superficie_terreno { get; set; }
        public DateTime? fecha_informe_tasacion { get; set; }
        public DateTime? fecha_solicitud_tasacion { get; set; }
        public DateTime? fecha_recepcion_tasacion { get; set; }
        public decimal? valor_tasacion_uf { get; set; }
        public decimal? valor_tasacion_pesos { get; set; }
        public decimal? valor_liquidacion_uf { get; set; }
        public decimal? valor_liquidacion_pesos { get; set; }
        public decimal? monto_asegurable_uf { get; set; }
        public decimal? monto_asegurable_pesos { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
