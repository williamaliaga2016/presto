using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class titular_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string? id_actividad { get; set; }
    public int numero_titular { get; set; }
    public string? tipo_identificacion { get; set; }
    public string? numero_identificacion { get; set; }
    public string? nombre_completo { get; set; }
    public string? celular_cliente { get; set; }
    public string? telefono_residente { get; set; }
    public string? email { get; set; }
    public string? direccion_residencia { get; set; }
    public string? telefono_declarativo { get; set; }
    public string? correo_declarativo { get; set; }
}
