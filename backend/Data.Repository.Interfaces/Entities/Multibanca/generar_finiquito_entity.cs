using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class generar_finiquito_entity
    {
        [Key]
        public int id_generar_finiquito { get; set; }
        public long id_expediente { get; set; }
        //propiedad
        public string fojas_propiedad { get; set; }
        public string numero_propiedad { get; set; }
        public string año_propiedad { get; set; }
        //hipoteca
        public string fojas_hipoteca { get; set; }
        public string numero_hipoteca { get; set; }
        public string año_hipoteca { get; set; }
        //prohibicion
        public string fojas_prohibicion { get; set; }
        public string numero_prohibicion { get; set; }
        public string año_prohibicion { get; set; }
        //Hipoteca 2do Grado
        public string fojas_hipoteca_2grado { get; set; }
        public string numero_hipoteca_2grado { get; set; }
        public string año_hipoteca_2grado { get; set; }
        public string? observaciones { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
