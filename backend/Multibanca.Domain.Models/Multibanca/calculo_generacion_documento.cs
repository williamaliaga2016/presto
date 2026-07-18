using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class calculo_generacion_documento : base_auditoria
    {
        public long id_calculo_generacion_documento { get; set; }
        public long id_expediente { get; set; }
        public string? tipo_propiedad { get; set; }
        public string? tipo_direccion { get; set; }
        public string? direccion { get; set; }
        public string? region { get; set; }
        public string? comuna { get; set; }
        public string? rol_avaluo { get; set; }
        public string? revision_rol_propiedad { get; set; }
        public decimal? valor_uf_fecha_hoy { get; set; }
        public DateTime? fecha_calculo { get; set; }
        public decimal? valor_uf_fecha_calculo { get; set; }
        public bool is_enviar_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
