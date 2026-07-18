using Data.Repository.Interfaces.Entities.FuncTransversal;

namespace Data.Repository.Interfaces.Repositories.FuncTransversal
{
    public interface IExpedienteDigitalMongoRepository
    {
        Task<List<expediente_digital_mongo_entity>> GetFilesByActividad(long idExpediente, string actividadId);
        Task<List<expediente_digital_mongo_entity>> GetFilesExpedienteDigital(long idExpediente, string? activityId = null);
        Task<expediente_digital_mongo_entity?> GetById(long idArchivo);
        Task<expediente_digital_mongo_entity> Create(expediente_digital_mongo_entity entity);
        Task<expediente_digital_mongo_entity?> Update(expediente_digital_mongo_entity entity);
        Task<int> GetNextVersion(long idExpediente, int idDocumento);
        Task<long> DeactivatePreviousVersions(long idExpediente, int idDocumento, long currentIdArchivo, int modifiedBy);
    }
}
