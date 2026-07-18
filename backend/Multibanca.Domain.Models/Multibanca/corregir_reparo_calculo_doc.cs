using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class corregir_reparo_calculo_doc : base_auditoria
    {
        public int id_corregir_reparo_calculo_doc { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public string? observaciones { get; set; }

        public string? existe_rol_avaluo { get; set; }

        public string? rol_avaluo_editado { get; set; }

        public decimal? valor_avaluo_pesos { get; set; }

        public string? solicitante { get; set; }

        public string? observaciones_reparo { get; set; }

        public DateTime? fecha_ingreso { get; set; }

        public string? tipo_propiedad { get; set; }

        public string? tipo_direccion { get; set; }

        public string? direccion { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? rol_avaluo { get; set; }
    }
}
