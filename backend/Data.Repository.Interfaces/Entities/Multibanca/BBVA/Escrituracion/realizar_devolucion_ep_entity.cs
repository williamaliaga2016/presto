using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion
{
    public class realizar_devolucion_ep_entity
    {
        [Key]
        public long      id                              { get; set; }
        public long      id_expediente                   { get; set; }
        public string?   id_actividad                    { get; set; }
        public string?   requiere_escalamiento_comercial { get; set; }
        public string?   tipologia                       { get; set; }
        public string?   casuistica                      { get; set; }
        public string?   accion_a_seguir                 { get; set; }
        public string?   observaciones                   { get; set; }

        public bool      is_active      { get; set; } = true;
        public bool      row_status     { get; set; } = true;
        public int       created_by     { get; set; }
        public DateTime  created_date   { get; set; } = DateTime.Now;
        public int?      modified_by    { get; set; }
        public DateTime? modified_date  { get; set; }
    }
}
