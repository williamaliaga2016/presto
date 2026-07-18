using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Security
{
    public class menus_entity
    {
        public menus_entity()
        {
            role_menus = new HashSet<role_menu_entity>();
        }

        [Key]
        public int menu_id { get; set; }
        public int? menu_padre_id { get; set; }
        public string name { get; set; }
        public string icon_name { get; set; }
        public string description_alt { get; set; }
        public string menu_url { get; set; }
        public bool? is_show_navbar { get; set; }
        public bool? is_show_home_menu { get; set; }
        public int orden { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public virtual ICollection<role_menu_entity> role_menus { get; set; }
    }
}
