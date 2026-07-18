using Common.Domain.Models;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using System;

namespace Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegalPostventa
{
    public class validacion_rectificatoria_legal_postventa : base_auditoria
    {
        public int id_validacion_rectificatoria_legal_postventa { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool is_subsanar { get; set; }
        public string? observaciones { get; set; }
        public string? solicitante { get; set; }
        public string? observaciones_reparo { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public string require_documentacion { get; set; }
        public string realiza_pago { get; set; }
        public bool encargado_firma { get; set; }
        public bool requiere_inscripcion_cbr { get; set; }
        public List<carga_operacion_banco_antecedente_comprador>? antecedentes_comprador { get; set; }
        public List<carga_operacion_banco_antecedente_vendedor>? antecedentes_vendedor { get; set; }

        public List<validacion_rectificatoria_legal_postventa_datos_personales>? validacion_rectificatoria_legal_postventa_datos_personales { get; set; }
    }
}
