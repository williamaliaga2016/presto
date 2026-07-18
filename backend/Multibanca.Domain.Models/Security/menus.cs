using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Security
{
    public class menus : base_auditoria
    {
        public int menu_id { get; set; }
        public int application_id { get; set; }
        public int? menu_padre_id { get; set; }
        public string name { get; set; }
        public string icon_name { get; set; }
        public string description_alt { get; set; }
        public string menu_url { get; set; }
        public bool? is_show_navbar { get; set; }
        public bool? is_show_home_menu { get; set; }
        public int orden { get; set; }

        public List<role_menu>? role_menus { get; set; }
    }
}
