using Data.Repository.Interfaces.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Security
{
    public interface IRoleMenuRepository : IMultibancaGenericRepository<role_menu_entity>, IDisposable
    {
        Task<List<role_menu_entity>> GetAllRoleMenuAssign(int role_id);
    }
}
