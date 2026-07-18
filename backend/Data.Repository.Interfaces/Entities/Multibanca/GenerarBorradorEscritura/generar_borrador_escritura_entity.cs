using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura
{
    public class generar_borrador_escritura_entity 
    {
        [Key]
        public int id_generar_borrador_escritura {  get; set; }
        public long id_expediente { get; set; }
        public bool existe_alzamiento { get; set; }
        public bool seguro_cesantia { get; set; }
        public bool mandato_judicial { get; set; }
        public string? beneficios { get; set; }
        public int id_notaria { get; set; }
        public bool enviar_reparo { get; set; }
        public string? observaciones { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
