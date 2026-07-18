using MongoDB.Bson.Serialization.Attributes;

namespace Data.Repository.Interfaces.Entities.FuncTransversal
{
    public class sequence_mongo_entity
    {
        [BsonId]
        public string _id { get; set; } = string.Empty;
        public long sequence_value { get; set; }
    }
}
