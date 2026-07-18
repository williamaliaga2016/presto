using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa
{
    public interface IValidacionRectificatoriaLegalPostventaRepository : IMultibancaGenericRepository<validacion_rectificatoria_legal_postventa_entity>, IDisposable
    {
        Task<validacion_rectificatoria_legal_postventa_entity> GetByExpediente(long id_expediente);
    }
}
