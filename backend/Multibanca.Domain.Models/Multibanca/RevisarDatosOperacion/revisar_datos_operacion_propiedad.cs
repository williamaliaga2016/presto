using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion
{
    public class revisar_datos_operacion_propiedad : base_auditoria
    {
        public int id_revisar_datos_operacion_propiedad { get; set; }

        public int id_revisar_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public string? tipo_propiedad { get; set; }

        public string? estado { get; set; }

        public string? tipo_venta { get; set; }

        public string? tipo_construccion { get; set; }

        public string? tipo_direccion { get; set; }

        public string? direccion { get; set; }

        public string? villa_condominio { get; set; }

        public string? numero { get; set; }

        public string? numero_casa_habitantes { get; set; }

        public string? conjunto { get; set; }

        public string? manzana { get; set; }

        public string? lote { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? existe_rol_avaluo { get; set; }

        public string? rol_avaluo_1 { get; set; }

        public string? rol_avaluo_2 { get; set; }

        public decimal? valor_avaluo_pesos { get; set; }

        public string? enviar_reparo { get; set; }

        public string? observaciones { get; set; }
    }
}
