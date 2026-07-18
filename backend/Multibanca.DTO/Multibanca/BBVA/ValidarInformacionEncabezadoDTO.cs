namespace Multibanca.DTO.Multibanca.BBVA;

public class ValidarInformacionEncabezadoDTO
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
