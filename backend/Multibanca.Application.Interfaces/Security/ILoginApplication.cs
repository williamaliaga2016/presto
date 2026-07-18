using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Security
{
    public interface ILoginApplication
    {
        Task<AuthUserDTO> Login(LoginDTO loginDTO);
        Task<users> GetUserByUserName(string user_name);
    }
}
