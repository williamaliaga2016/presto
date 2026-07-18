using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco: base_auditoria
    {
        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        // ============================================================
        // SECCIONES HIJAS DE LA ACTIVIDAD
        // ============================================================

        public carga_operacion_banco_datos_operacion? datos_operacion { get; set; }        
        public List<carga_operacion_banco_antecedente_comprador>? antecedentes_comprador { get; set; }
        public List<carga_operacion_banco_antecedente_vendedor>? antecedentes_vendedor { get; set; }
        public carga_operacion_banco_antecedente_credito? antecedente_credito { get; set; }
        public carga_operacion_banco_datos_comercial? datos_comercial { get; set; }
    }
}
