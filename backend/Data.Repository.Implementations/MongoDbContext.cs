using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Multibanca.Common.Settings;

namespace Data.Repository.Implementations
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase MongoDatabase;
        private readonly MongoDbSettings Settings;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            Settings = mongoDbSettings.Value;

            MongoClient mongoClient = new MongoClient(Settings.ConnectionString);
            MongoDatabase = mongoClient.GetDatabase(Settings.DatabaseName);
        }

        public IMongoCollection<expediente_digital_mongo_entity> ExpedienteDigital =>
            MongoDatabase.GetCollection<expediente_digital_mongo_entity>(
                Settings.ExpedienteDigitalCollectionName
            );

        public IMongoCollection<sequence_mongo_entity> Sequences =>
            MongoDatabase.GetCollection<sequence_mongo_entity>(
                Settings.SequenceCollectionName
            );
    }
}
