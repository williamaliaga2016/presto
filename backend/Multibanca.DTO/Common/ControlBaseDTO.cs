using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Common
{
    public class ControlBaseDTO
    {
        public int id { get; set; }
        public long idBig { get; set; }
        public string? code { get; set; }
        public string? description { get; set; }
        public string? parent_code { get; set; }
    }
}
