using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Multibanca
{
    public class EncabezadoDTO
    {
        public long id_expediente { get; set; }
        public string responsable { get; set; }
        public string actividad { get; set; }
        public string estado { get; set; }
        public string usuario_asignado { get; set; }
        public DateTime? fecha_alta { get; set; }
        public DateTime? fecha_asignacion { get; set; }

        // Campos legados de Chile aún consumidos por otros flujos BBVA
        // (ValidarIntegracion/AsignarFirmas/ValidarInformacion/DevolucionVbComercial).
        // plazo se sobreescribe en EncabezadoApplication.PoblarCamposBbva con el valor
        // de carga_operacion_banco_antecedente_credito para expedientes BBVA Colombia.
        public int plazo { get; set; }
        public decimal tasa { get; set; }

        // ============================================================
        // CAMPOS BBVA COLOMBIA — añadidos para Presto Legalización
        // ============================================================
        public string? id_scoring { get; set; }
        public string? numero_identificacion_t1 { get; set; }
        public string? tipo_documento_id_t1 { get; set; }
        public string? nombre_completo_t1 { get; set; }
        public string? celular_t1 { get; set; }
        public string? numero_identificacion_t2 { get; set; }
        public string? nombre_completo_t2 { get; set; }
        public bool? cliente_nomina { get; set; }
        public string? situacion_laboral { get; set; }
        public string? correo_declarativo { get; set; }
        public string? telefono_declarativo { get; set; }
        public string? codigo_oficina_bbva { get; set; }
        public string? descripcion_oficina_bbva { get; set; }
        public string? codigo_asesor_bbva { get; set; }
        public string? id_tipo_sub_producto { get; set; }
        public decimal? monto_otorgado { get; set; }
        public string? canal_originacion { get; set; }
        public DateTime? fecha_aprobacion { get; set; }
        public string? codigo_proyecto { get; set; }
        public string? descripcion_proyecto { get; set; }
        public string? estado_inmueble { get; set; }
        public string? tipo_inmueble { get; set; }
        public string? condiciones_organismo_decisor { get; set; }
    }
}
