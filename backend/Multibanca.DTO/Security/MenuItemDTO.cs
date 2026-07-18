using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Security
{
    public class MenuItemDTO
    {
        public int menuId { get; set; }
        public string name { get; set; } = string.Empty;
        public string? menuUrl { get; set; }
        public int? menuPadreId { get; set; }
        public sbyte disabled { get; set; }
        public sbyte is_active { get; set; }
        public List<MenuItemDTO> children { get; set; } = new();
    }
}
