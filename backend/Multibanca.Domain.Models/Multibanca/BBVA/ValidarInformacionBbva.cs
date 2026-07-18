using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class validar_informacion_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "ACT_VALIDAR_INFO";
    // Titular T1
    public string? tipo_id_t1 { get; set; }
    public string? numero_id_t1 { get; set; }
    public string? nombre_completo_t1 { get; set; }
    public string? celular_t1 { get; set; }
    public string? telefono_t1 { get; set; }
    public string? email_t1 { get; set; }
    public string? direccion_t1 { get; set; }
    public string? departamento_t1 { get; set; }
    public string? municipio_t1 { get; set; }
    public string? situacion_laboral_t1 { get; set; }
    public bool? cliente_nomina_t1 { get; set; }
    // Titular T2
    public string? tipo_id_t2 { get; set; }
    public string? numero_id_t2 { get; set; }
    public string? nombre_completo_t2 { get; set; }
    public string? celular_t2 { get; set; }
    public string? email_t2 { get; set; }
    // Titular T3
    public string? tipo_id_t3 { get; set; }
    public string? numero_id_t3 { get; set; }
    public string? nombre_completo_t3 { get; set; }
    public string? celular_t3 { get; set; }
    public string? email_t3 { get; set; }
    // Inmueble
    public bool? inmueble_definido { get; set; }
    public string? tipo_inmueble { get; set; }
    public string? estado_inmueble { get; set; }
    public string? constructora { get; set; }
    public bool? es_constructora_vip { get; set; }
    public string? codigo_proyecto { get; set; }
    public string? descripcion_proyecto { get; set; }
    public string? departamento_inmueble { get; set; }
    public string? municipio_inmueble { get; set; }
    public DateTime? fecha_estimada_entrega { get; set; }
    // Estatus
    public string? estatus_general { get; set; } = "SIN_INM";
    public string? origen_devolucion { get; set; }
    // Flags decisión Avanzar
    public bool? requiere_definir_inmueble { get; set; }
    public bool? requiere_carga_cliente { get; set; }
    public bool? requiere_carga_constructora { get; set; }
    // Datos del Crédito
    public string? tipo_credito { get; set; }
    public bool? tiene_garantia { get; set; }
    public bool? garantia_constituida { get; set; }
    public decimal? monto_otorgado_vi { get; set; }
    public decimal? monto_otorgado_vivienda_original { get; set; }
    // Datos de contacto declarativo
    public string? correo_declarativo { get; set; }
    public string? telefono_declarativo { get; set; }
    public string? codigo_oficina { get; set; }
    public string? descripcion_oficina { get; set; }
    public string? codigo_asesor { get; set; }
    // Datos comerciales / devolución
    public string? motivo_devolucion { get; set; }
    public string? observaciones { get; set; }
}
