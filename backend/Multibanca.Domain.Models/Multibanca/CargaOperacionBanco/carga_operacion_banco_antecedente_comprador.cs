using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_antecedente_comprador : base_auditoria
    {
        public int id_carga_operacion_banco_antecedente_comprador { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public string? rut { get; set; }

        public string? tipo_comprador { get; set; }

        public string? razon_social { get; set; }

        public string? nombres { get; set; }

        public string? apellido_paterno { get; set; }

        public string? apellido_materno { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        public string? genero { get; set; }

        public string? estado_civil { get; set; }

        public string? relacion_titular { get; set; }

        public string? direccion { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? telefono { get; set; }

        public string? email { get; set; }

        public string? nacionalidad { get; set; }

        public string? profesion { get; set; }

        // ============================================================
        // CAMPOS BBVA COLOMBIA — agregados para Presto Legalización
        // Los campos de Chile (rut, region, comuna) se mantienen por
        // compatibilidad. Usar estos para Colombia.
        // ============================================================
        public string? numero_identificacion { get; set; }  // reemplaza rut
        public string? tipo_documento_id { get; set; }       // CC, CE, PA, NIT
        public string? nombre_completo { get; set; }         // nombre completo sin split
        public string? celular { get; set; }                 // celular Colombia
        public string? departamento { get; set; }            // reemplaza region
        public string? municipio { get; set; }               // reemplaza comuna
        public string? situacion_laboral { get; set; }       // catálogo SITUACION_LABORAL
        public bool? cliente_nomina { get; set; }            // flag nómina BBVA
        public string? tipo_titular { get; set; }            // T1, T2, T3
    }
}
