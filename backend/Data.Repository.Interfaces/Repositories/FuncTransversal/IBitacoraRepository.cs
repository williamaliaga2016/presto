using Data.Repository.Interfaces.Entities.FuncTransversal;

namespace Data.Repository.Interfaces.Repositories.FuncTransversal
{
    public interface IBitacoraRepository : IMultibancaGenericRepository<bitacora_entity>
    {
        Task<bitacora_entity?> GetByExpedienteActividad(long id_expediente, string id_actividad);
    }
}