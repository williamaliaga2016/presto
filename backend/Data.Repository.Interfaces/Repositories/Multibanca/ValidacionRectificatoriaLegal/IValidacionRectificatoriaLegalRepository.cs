using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;

namespace Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalRepository : IMultibancaGenericRepository<validacion_rectificatoria_legal_entity>, IDisposable
    {
        Task<validacion_rectificatoria_legal_entity> GetByExpediente(long id_expediente);
    }
}
