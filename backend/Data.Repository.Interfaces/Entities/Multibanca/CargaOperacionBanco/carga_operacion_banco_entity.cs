using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_entity
    {
        [Key]
        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
