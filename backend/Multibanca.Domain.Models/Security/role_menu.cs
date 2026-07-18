using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Security
{
    public class role_menu : base_auditoria
    {
        public int role_menu_id { get; set; }
        public int role_id { get; set; }
        public int menu_id { get; set; }

        public roles role { get; set; }
        public menus menu { get; set; }
    }
}
