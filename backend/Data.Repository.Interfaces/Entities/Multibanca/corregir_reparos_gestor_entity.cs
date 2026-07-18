using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class corregir_reparos_gestor_entity
    {
        [Key]
        public int id_corregir_reparos_gestor { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool is_subsanar { get; set; }
        public string? observaciones { get; set; }
        public string? estatus_general { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
