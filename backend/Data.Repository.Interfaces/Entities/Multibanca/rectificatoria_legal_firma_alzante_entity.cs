using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class rectificatoria_legal_firma_alzante_entity
    {
        [Key]
        public int id_rectificatoria_legal_firma_alzante { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public DateTime? fecha_firma_alzante { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
