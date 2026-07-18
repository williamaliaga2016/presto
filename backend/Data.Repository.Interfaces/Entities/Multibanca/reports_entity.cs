namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class reports_entity
    {
        public int id_reporte { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public string report_path { get; set; } = string.Empty;
        public string? template { get; set; }
        public string? extension { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
