using Data.Repository.Interfaces.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Security
{
    public interface IUserRepository : IMultibancaGenericRepository<users_entity>, IDisposable
    {
    }
}
