using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura
{
    public class generar_borrador_escritura_detalle_entity
    {
        [Key]
        public int id_generar_borrador_escritura_detalle_entity { get; set; }
        public int id_generar_borrador_escritura { get; set; }
        public int id_datos_operacion_fiador_garante { get; set; } //Viene de datos_operacion_fiador_garante_entity
        public long id_expediente { get; set; }
        public int id_rol_comparecencia { get; set; }
        public bool requiere_firma {  get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }

    }
}
