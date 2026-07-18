using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IAsignarEstudioTitulosRepository : IMultibancaGenericRepository<asignar_estudio_titulos_entity>, IDisposable
    {
        Task<asignar_estudio_titulos_entity?> GetByExpedienteActividad(long id_expediente, string id_actividad);
    }
}

