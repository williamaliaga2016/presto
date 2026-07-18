using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Repository.Interfaces.Entities.FuncTransversal
{
    public class expediente_digital_mongo_entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }

        public long id_archivo { get; set; }
        public long id_expediente { get; set; }
        public int id_documento { get; set; }
        public int? id_usuario { get; set; }

        public string nombre_archivo { get; set; } = string.Empty;
        public string nombre_archivo_original { get; set; } = string.Empty;
        public string extension { get; set; } = string.Empty;

        public string? mime_type { get; set; }
        public long? file_size { get; set; }

        public int version_archivo { get; set; }
        public DateTime? fecha_alta { get; set; }
        public string comentarios { get; set; } = string.Empty;

        public string storage_provider { get; set; } = "local";
        public string storage_path { get; set; } = string.Empty;
        public string? activity_id { get; set; }

        public Dictionary<string, object>? metadata_extra { get; set; }

        public bool is_active { get; set; }
        public bool row_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
    }
}
