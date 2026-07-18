using Data.Repository.Interfaces.Entities.FuncTransversal;
using MongoDB.Driver;

namespace Data.Repository.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<expediente_digital_mongo_entity> ExpedienteDigital { get; }
        IMongoCollection<sequence_mongo_entity> Sequences { get; }
    }
}
