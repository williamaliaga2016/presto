using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IAsignarEscrituraRepository : IMultibancaGenericRepository<asignar_escritura_entity>, IDisposable
    {
        Task<asignar_escritura_entity?> GetByExpedienteActividad(long id_expediente, string id_actividad);
    }
}
