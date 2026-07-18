using Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Security
{
    public class roles : base_auditoria
    {
        public int role_id { get; set; }
        public string? code { get; set; }
        public string name { get; set; }

        public List<users>? users { get; set; }
        public List<role_menu>? role_menus { get; set; }
    }
}
