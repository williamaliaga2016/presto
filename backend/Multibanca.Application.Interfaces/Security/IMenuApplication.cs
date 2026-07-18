using Common.Application.Interfaces;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Security
{
    public interface IMenuApplication : IMultibancaGenericApplication<menus>
    {
        Task<List<MenuItemDTO>> GetMenuJerarquico();
        Task<List<MenuItemDTO>> GetMenuByUserId(int userId);
        Task<List<MenuItemDTO>> GetMenuByRoleId(int role_id);
    }
}
