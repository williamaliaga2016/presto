using Data.Repository.Interfaces.Entities.Security;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Security
{
    public interface IMenuRepository : IMultibancaGenericRepository<menus_entity>, IDisposable
    {
        Task<List<MenuItemDTO>> GetMenuJerarquico();
        Task<List<MenuItemDTO>> GetMenuByUserId(int userId);
        Task<List<MenuItemDTO>> GetMenuByRoleId(int role_id);
    }
}
