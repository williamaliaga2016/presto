using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Security
{
    public class role_menu_entity
    {
        [Key]
        public int role_menu_id { get; set; }
        public int role_id { get; set; }
        public int menu_id { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public virtual roles_entity? role { get; set; }
        public virtual menus_entity? menu { get; set; }
    }
}
