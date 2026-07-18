using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class aprobacion_comercial_legal_cdr_entity
    {
        [Key]
        public int id_aprobacion_comercial_legal_cdr { get; set; }
        public long id_expediente { get; set; }
        public bool enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
