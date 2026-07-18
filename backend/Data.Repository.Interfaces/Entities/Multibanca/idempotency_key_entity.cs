using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class idempotency_key_entity
    {
        [Key]
        public string key { get; set; } = null!;
        public long id_expediente { get; set; }
        public string response_snapshot { get; set; } = null!;
        public DateTime created_at { get; set; }
    }
}
