using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Security
{
    public class RoleMenuRepository : MultibancaGenericRepository<role_menu_entity>, IRoleMenuRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;
        public RoleMenuRepository(MultibancaDBContext _skayrosDBContext) : base(_skayrosDBContext)
        {
            MultibancaDBContext = _skayrosDBContext;
        }

        public async Task<List<role_menu_entity>> GetAllRoleMenuAssign(int role_id)
        {
            return await MultibancaDBContext.Set<role_menu_entity>()
                .Where(q => q.row_status && q.role_id == role_id)
                .ToListAsync();
        }
    }
}
