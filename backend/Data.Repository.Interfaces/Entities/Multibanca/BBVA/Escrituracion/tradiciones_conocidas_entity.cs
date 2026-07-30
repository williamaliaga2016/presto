using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion
{
    public class tradiciones_conocidas_entity
    {
        [Key]
        public long      id                          { get; set; }
        public string?   ciudad_del_inmueble         { get; set; }
        public string    codigo_proyecto             { get; set; } = string.Empty;
        public string?   proyecto                    { get; set; }
        public string?   constructora                { get; set; }
        public string?   firma                       { get; set; }
        public string?   numero_identificacion_firma { get; set; }
        public string?   nit_abogado                 { get; set; }
        public string?   nombre_abogado              { get; set; }
        public string?   vip                         { get; set; }
        public DateTime? fecha_creacion_modificacion { get; set; }
        public string?   proyecto_av_tipo            { get; set; }
        public string?   tipo_desembolso             { get; set; }
        public string?   correo_constructora         { get; set; }
        public bool      is_active                   { get; set; } = true;
        public bool      row_status                  { get; set; } = true;
        public int       created_by                  { get; set; } = 1;
        public DateTime  created_date                { get; set; } = DateTime.Now;
    }
}
