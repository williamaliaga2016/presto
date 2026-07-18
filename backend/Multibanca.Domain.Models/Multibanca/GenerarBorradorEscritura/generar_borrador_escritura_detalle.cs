using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura
{
    public class generar_borrador_escritura_detalle : base_auditoria
    {
        public int id_generar_borrador_escritura_detalle_entity { get; set; }
        public int id_generar_borrador_escritura { get; set; }
        public int id_datos_operacion_fiador_garante { get; set; } //Viene de datos_operacion_fiador_garante_entity
        public long id_expediente { get; set; }
        public int id_rol_comparecencia { get; set; }
        public bool requiere_firma { get; set; }
    }
}
