using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;

namespace Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalDatosPersonalesRepository : IMultibancaGenericRepository<validacion_rectificatoria_legal_datos_personales_entity>, IDisposable
    {
        Task<List<validacion_rectificatoria_legal_datos_personales_entity>> GetByExpediente(long id_expediente);
    }
}
