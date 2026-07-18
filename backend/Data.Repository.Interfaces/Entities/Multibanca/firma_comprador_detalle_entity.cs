using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class firma_comprador_detalle_entity
    {
        [Key]
        public int id_firma_comprador_detalle { get; set; }
        public int id_firma_comprador { get; set; }
        public long id_expediente { get; set; }
        public string? relacion_titular { get; set; } = string.Empty;
        public string? rut { get; set; } = string.Empty;
        public string? nombres { get; set; } = string.Empty;
        public string? apellido_paterno { get; set; } = string.Empty;
        public string? apellido_materno { get; set; } = string.Empty;
        public string? estado_civil { get; set; } = string.Empty;
        public DateTime fecha_firma { get; set; }

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
