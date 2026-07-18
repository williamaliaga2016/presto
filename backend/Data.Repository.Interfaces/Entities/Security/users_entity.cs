using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Security
{
    public class users_entity
    {
        [Key]
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string name { get; set; }
        public string? last_name_first { get; set; }
        public string? last_name_second { get; set; }
        public int id_document_type { get; set; }
        public string? nro_document { get; set; }
        public string? user_name { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public bool? is_first_access { get; set; }
        public int remaining_attempts { get; set; }
        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public virtual roles_entity? role { get; set; }
    }
}
