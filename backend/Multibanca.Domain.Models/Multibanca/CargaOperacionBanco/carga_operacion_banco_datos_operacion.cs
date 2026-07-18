using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_datos_operacion : base_auditoria
    {
        public int id_carga_operacion_banco_datos_operacion { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public long? nro_mutuo { get; set; }

        public string? tipo_operacion { get; set; }

        public long? nro_registro { get; set; }

        public string? ult_clasif_al { get; set; }

        public string? segmento { get; set; }

        public string? canal_venta { get; set; }

        public long? nro_op_cartera { get; set; }

        public string? modelo_operacion { get; set; }

        public string? tipo_carpeta { get; set; }

        public string? propietario { get; set; }

        public string? inmobiliaria { get; set; }

        public string? glosa_producto { get; set; }

        public string? codigo_producto_comercial { get; set; }

        public int? nro_piloto { get; set; }

        public string? banco_alzante { get; set; }

        public string? nombre_proyecto { get; set; }

        // ============================================================
        // CAMPOS BBVA COLOMBIA
        // ============================================================
        public string? id_scoring { get; set; }           // reemplaza nro_mutuo
        public string? codigo_asesor { get; set; }        // código asesor BBVA
        public string? codigo_oficina { get; set; }       // código oficina (ej: 0158)
        public string? descripcion_oficina { get; set; }  // nombre oficina
        public string? canal_originacion { get; set; }    // reemplaza canal_venta
        public string? tipo_inmueble { get; set; }        // NUEVA, USADA, SOBRE_PLANOS
        public string? estado_inmueble { get; set; }      // estado del inmueble
        public string? descripcion_estado_inmueble { get; set; } // glosa del estado del inmueble
        public string? codigo_proyecto { get; set; }      // código proyecto constructor
        public string? descripcion_proyecto { get; set; } // reemplaza nombre_proyecto
    }
}
