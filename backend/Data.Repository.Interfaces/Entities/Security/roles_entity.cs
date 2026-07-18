using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Security
{
    public class roles_entity
    {
        public roles_entity()
        {
            role_menus = new HashSet<role_menu_entity>();
        }

        [Key]
        public int role_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public virtual ICollection<users_entity> users { get; set; }
        public virtual ICollection<role_menu_entity> role_menus { get; set; }
    }
}
