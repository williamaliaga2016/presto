using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class actividades_entity
    {
        [Key]
        public long id { set; get; }
        public long id_expediente { set; get; }
        public string id_actividad { set; get; }
        public long id_rol { set; get; }
        public string status { set; get; }
        public int id_usuario { set; get; }
        public string descripcion { set; get; }
        public bool activo { set; get; }
        public DateTime? fecha_alta { set; get; }
        public DateTime? fecha_asignacion { set; get; }
        public DateTime? fecha_inicio { set; get; }
        public DateTime? fecha_termino { set; get; }
        public DateTime? fecha_cancelacion { set; get; }
        public DateTime? fecha_actualizacion { set; get; }
        public DateTime? fecha_reingreso { set; get; }        
        public DateTime? fecha_suspencion { get; set; }
        public DateTime? fecha_reactivacion { get; set; }
    }
}
