using Data.Repository.Interfaces.Entities.Security;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Security
{
    public interface ILoginRepository
    {
        Task<users_entity> Login(LoginDTO loginDTO);
        Task<users_entity> GetUserByUserName(string user_name);
    }
}
