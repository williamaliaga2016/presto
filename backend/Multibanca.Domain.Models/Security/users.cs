using Common.Domain.Models;
using System.Data;

namespace Multibanca.Domain.Models.Security
{
    public class users : base_auditoria
    {
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string? name { get; set; }
        public string? last_name_first { get; set; }
        public string? last_name_second { get; set; }
        public int id_document_type { get; set; }
        public string? nro_document { get; set; }
        public string? user_name { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public bool? is_first_access { get; set; }
        public int remaining_attempts { get; set; }
        public roles? role { get; set; }

        // Extras
        public string? name_complete { get; set; }
        public string? role_name { get; set; }
    }
}
