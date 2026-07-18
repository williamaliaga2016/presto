using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.DTO.Security
{
    public class AuthUserDTO
    {
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string code { get; set; }//preformer
        public string? user_name { get; set; }
        public string? role_name { get; set; }
        public string? name_complete { get; set; }
        public string? email { get; set; }
        public string? token_multibanca { get; set; }
        public string? token_captcha { get; set; }
        public bool? status { get; set; }
        public string? message { get; set; }
    }
}
