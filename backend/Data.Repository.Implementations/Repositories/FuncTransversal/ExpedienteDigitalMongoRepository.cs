using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using MongoDB.Driver;

namespace Data.Repository.Implementations.Repositories.FuncTransversal
{
    public class ExpedienteDigitalMongoRepository : IExpedienteDigitalMongoRepository
    {
        private readonly IMongoDbContext MongoDbContext;

        public ExpedienteDigitalMongoRepository(IMongoDbContext mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        /// <summary>
        /// Consulta archivos del expediente digital y aplica filtro por actividad cuando la vista debe limitar la exposicion documental.
        /// </summary>
        /// <param name="idExpediente">Identificador del expediente.</param>
        /// <param name="activityId">Actividad workflow usada para limitar el listado.</param>
        /// <returns>Archivos activos del expediente que cumplen el filtro solicitado.</returns>
        public async Task<List<expediente_digital_mongo_entity>> GetFilesExpedienteDigital(long idExpediente, string? activityId = null)
        {
            FilterDefinition<expediente_digital_mongo_entity> filter =
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.id_expediente, idExpediente) &
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.row_status, true);

            if (!string.IsNullOrWhiteSpace(activityId))
            {
                filter &= Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.activity_id, activityId);
            }
            try
            {
                return await MongoDbContext.ExpedienteDigital
                                .Find(filter)
                                .SortByDescending(x => x.id_archivo)
                                .ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<List<expediente_digital_mongo_entity>> GetFilesByActividad(long idExpediente, string actividadId)
        {
            return await MongoDbContext.ExpedienteDigital
                .Find(x => x.id_expediente == idExpediente &&
                           x.activity_id == actividadId &&
                           x.row_status == true)
                .SortByDescending(x => x.id_archivo)
                .ToListAsync();
        }

        public async Task<expediente_digital_mongo_entity?> GetById(long idArchivo)
        {
            return await MongoDbContext.ExpedienteDigital
                .Find(x => x.id_archivo == idArchivo &&
                           x.row_status == true)
                .FirstOrDefaultAsync();
        }

        public async Task<expediente_digital_mongo_entity> Create(expediente_digital_mongo_entity entity)
        {
            entity.id_archivo = await GetNextSequence("expediente_digital");

            await MongoDbContext.ExpedienteDigital.InsertOneAsync(entity);

            return entity;
        }

        public async Task<expediente_digital_mongo_entity?> Update(expediente_digital_mongo_entity entity)
        {
            FilterDefinition<expediente_digital_mongo_entity> filter =
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.id_archivo, entity.id_archivo);

            await MongoDbContext.ExpedienteDigital.ReplaceOneAsync(filter, entity);

            return await GetById(entity.id_archivo);
        }

        public async Task<int> GetNextVersion(long idExpediente, int idDocumento)
        {
            List<expediente_digital_mongo_entity> documents = await MongoDbContext.ExpedienteDigital
                .Find(x => x.id_expediente == idExpediente &&
                           x.id_documento == idDocumento &&
                           x.row_status == true)
                .ToListAsync();

            if (documents == null || documents.Count == 0)
            {
                return 1;
            }

            return documents.Max(x => x.version_archivo) + 1;
        }

        public async Task<long> DeactivatePreviousVersions(long idExpediente, int idDocumento, long currentIdArchivo, int modifiedBy)
        {
            FilterDefinition<expediente_digital_mongo_entity> filter =
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.id_expediente, idExpediente) &
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.id_documento, idDocumento) &
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.row_status, true) &
                Builders<expediente_digital_mongo_entity>.Filter.Eq(x => x.is_active, true) &
                Builders<expediente_digital_mongo_entity>.Filter.Ne(x => x.id_archivo, currentIdArchivo);

            UpdateDefinition<expediente_digital_mongo_entity> update =
                Builders<expediente_digital_mongo_entity>.Update
                    .Set(x => x.is_active, false)
                    .Set(x => x.modified_by, modifiedBy)
                    .Set(x => x.modified_date, DateTime.Now);

            UpdateResult result = await MongoDbContext.ExpedienteDigital.UpdateManyAsync(filter, update);

            return result.ModifiedCount;
        }

        private async Task<long> GetNextSequence(string sequenceName)
        {
            FilterDefinition<sequence_mongo_entity> filter =
                Builders<sequence_mongo_entity>.Filter.Eq(x => x._id, sequenceName);

            UpdateDefinition<sequence_mongo_entity> update =
                Builders<sequence_mongo_entity>.Update.Inc(x => x.sequence_value, 1);

            FindOneAndUpdateOptions<sequence_mongo_entity> options = new FindOneAndUpdateOptions<sequence_mongo_entity>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            sequence_mongo_entity result = await MongoDbContext.Sequences
                .FindOneAndUpdateAsync(filter, update, options);

            return result.sequence_value;
        }
    }
}
