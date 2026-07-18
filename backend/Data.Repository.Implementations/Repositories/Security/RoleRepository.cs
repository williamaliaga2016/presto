using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Security
{
    public class RoleRepository : MultibancaGenericRepository<roles_entity>, IRoleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;
        public RoleRepository(MultibancaDBContext _skayrosDBContext) : base(_skayrosDBContext)
        {
            MultibancaDBContext = _skayrosDBContext;
        }

        public async Task<List<ControlBaseDTO>> GetControlRole()
        {
            return await MultibancaDBContext.Set<roles_entity>()
                .Where(q => q.row_status && q.is_active)
                .Select(q => new ControlBaseDTO
                {
                    id = q.role_id,
                    description = q.name
                }).ToListAsync();
        }
    }
}
