using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class firma_vendedor_detalle : base_auditoria
    {
        public int id_firma_vendedor_detalle { get; set; }
        public int id_firma_vendedor { get; set; }
        public long id_expediente { get; set; }
        public string? relacion_titular { get; set; }
        public string? rut { get; set; }
        public string? nombres { get; set; }
        public string? apellido_paterno { get; set; }
        public string? apellido_materno { get; set; }
        public string? estado_civil { get; set; }
        public DateTime? fecha_firma { get; set; }
    }
}
