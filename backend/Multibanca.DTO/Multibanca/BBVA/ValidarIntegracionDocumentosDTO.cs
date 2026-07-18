namespace Multibanca.DTO.Multibanca.BBVA
{

    // DTO DE LECTURA (RESPONSE)
    public class ValidarIntegracionDocumentosDTO
    {
        public long id_expediente { get; set; }
        public ValidarIntegracionDocumentosEncabezadoDTO encabezado { get; set; }
        public ValidarIntegracionDocumentosFormularioDTO formulario { get; set; }
        public object validar_informacion_data { get; set; }
    }

    // DTO DE ESCRITURA (REQUEST DE GUARDADO)
    public class GuardarValidarIntegracionDocumentosDTO
    {
        public ValidarIntegracionDocumentosFormularioDTO formulario { get; set; }
        public object validar_informacion_data { get; set; }
    }

    // DTO DE COMPOSICION (FORMULARIO PRINCIPAL)
    public class ValidarIntegracionDocumentosFormularioDTO
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_VALIDAR_INTEGRACION";
        public bool? documentos_correctos { get; set; }
        public bool credito_condicionado { get; set; } = false;
        public string? motivo_devolucion { get; set; }
        public string? observaciones { get; set; }
    }

    // DTO DE COMPOSICION (ENCABEZADO DE SOLO LECTURA)
    public class ValidarIntegracionDocumentosEncabezadoDTO
    {
        public string? scoring { get; set; }
        public string? tipo_subproducto { get; set; }
        public decimal? monto_otorgado_original { get; set; }
        public int plazo_meses { get; set; }
        public decimal tasa { get; set; }
        public string? condiciones_organismo_decisor { get; set; }
        public string? codigo_oficina { get; set; }
        public string? descripcion_oficina { get; set; }
        public string? codigo_asesor { get; set; }
        public string? correo_declarativo_original { get; set; }
        public string? telefono_declarativo_original { get; set; }
    }

    #region INTERVINIENTE

    public class IntervinienteDTO
    {
        public int? id { get; set; }
        public long id_expediente { get; set; }
        public string? nombre_completo { get; set; }
        public string? tipo_identificacion { get; set; }
        public string? tipo_identificacion_descripcion { get; set; }
        public string? numero_identificacion { get; set; }
        public string? telefono { get; set; }
        public string? correo_electronico { get; set; }
    }

    #endregion
}
