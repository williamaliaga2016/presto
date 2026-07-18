using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class corregir_reparo_calculo_doc_entity
    {
        [Key]
        public int id_corregir_reparo_calculo_doc { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public string? observaciones { get; set; }

        public string? existe_rol_avaluo { get; set; }

        public string? rol_avaluo_editado { get; set; }

        public decimal? valor_avaluo_pesos { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
