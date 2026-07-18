using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Security
{
    public class LoginDTO
    {
        public int userId { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string? newPassword { get; set; }
        public string? confirmPassword { get; set; }
        public string? forgotEmail { get; set; }
    }
}
