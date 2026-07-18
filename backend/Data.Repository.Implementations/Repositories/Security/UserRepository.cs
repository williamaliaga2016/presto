using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Security
{
    public class UserRepository : MultibancaGenericRepository<users_entity>, IUserRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;
        public UserRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }
    }
}
