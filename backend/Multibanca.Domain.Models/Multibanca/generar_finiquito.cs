
using Common.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_finiquito : base_auditoria
    {
        public int id_generar_finiquito { get; set; }
        public long id_expediente { get; set; }

        // Propiedad
        public string fojas_propiedad { get; set; }
        public string numero_propiedad { get; set; }
        public string año_propiedad { get; set; }

        // Hipoteca
        public string fojas_hipoteca { get; set; }
        public string numero_hipoteca { get; set; }
        public string año_hipoteca { get; set; }

        // Prohibicion
        public string fojas_prohibicion { get; set; }
        public string numero_prohibicion { get; set; }
        public string año_prohibicion { get; set; }

        // Hipoteca 2do Grado
        public string fojas_hipoteca_2grado { get; set; }
        public string numero_hipoteca_2grado { get; set; }
        public string año_hipoteca_2grado { get; set; }

        public string? observaciones { get; set; }

        //Campos no Mapeados
        [NotMapped]
        public string tipo_tasacion { get; set; }
        [NotMapped]
        public string? tipo_propiedad { get; set; }
        [NotMapped]
        public string? direccion { get; set; }

        [NotMapped]
        public string? comuna { get; set; }

        [NotMapped]
        public string? rol_avaluo { get; set; }

        [NotMapped]
        public DateTime? fecha_informe_tasacion { get; set; }

        [NotMapped]
        public DateTime? fecha_solicitud_tasacion { get; set; }

        [NotMapped]
        public DateTime? fecha_recepcion_tasacion { get; set; }
    }
}
