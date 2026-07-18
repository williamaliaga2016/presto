namespace Multibanca.DTO.Multibanca.BBVA
{
    namespace Multibanca.Application.DTOs
    {
        // DTO DE LECTURA (RESPONSE)
        public class DevolucionVbComercialDTO
        {
            public DevolucionVbComercialEncabezadoDTO encabezado { get; set; } = new();
            public DevolucionVbComerciaFormularioDTO formulario { get; set; } = new();
            public object validar_informacion_data { get; set; }
        }

        // DTO DE ESCRITURA (REQUEST DE GUARDADO)
        public class GuardarDevolucionVbComercialDTO
        {
            public DevolucionVbComerciaFormularioDTO formulario { get; set; }
        }

        // DTO DE COMPOSICION (FORMULARIO PRINCIPAL)
        public class DevolucionVbComerciaFormularioDTO
        {
            public int id { get; set; }
            public long id_expediente { get; set; }
            public string id_actividad { get; set; } = "ACT_DEVOLUCION_VB_COMERCIAL";
            public bool? cliente_desiste { get; set; }
            public string? motivo_cierre { get; set; }
            public string? observaciones { get; set; }
        }

        // DTO DE COMPOSICION (ENCABEZADO DE SOLO LECTURA)
        public class DevolucionVbComercialEncabezadoDTO
        {
            public string? scoring { get; set; }
            public string? tipo_subproducto { get; set; }
            public decimal? monto_otorgado_original { get; set; }
            public int? plazo_meses { get; set; }
            public decimal? tasa { get; set; }
            public string? condiciones_organismo_decisor { get; set; }
            public string? codigo_oficina { get; set; }
            public string? descripcion_oficina { get; set; }
            public string? codigo_asesor { get; set; }
            public string? correo_declarativo_original { get; set; }
            public string? telefono_declarativo_original { get; set; }
        }
    }
}
