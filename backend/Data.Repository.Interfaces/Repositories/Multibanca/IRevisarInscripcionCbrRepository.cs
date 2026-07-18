using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarInscripcionCbrRepository : IMultibancaGenericRepository<revisar_inscripcion_cbr_entity>, IDisposable
    {
        Task<revisar_inscripcion_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
