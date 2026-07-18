using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class asignar_estudio_titulos_entity
    {
        [Key]
        public int id_asignar_estudio_titulos { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = string.Empty;
        public string abogado { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}

