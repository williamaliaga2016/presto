using Data.Repository.Interfaces.Entities.Security;
using Multibanca.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Security
{
    public interface IRoleRepository : IMultibancaGenericRepository<roles_entity>, IDisposable
    {
        Task<List<ControlBaseDTO>> GetControlRole();
    }
}
