using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa
{
    public interface IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository : IMultibancaGenericRepository<validacion_rectificatoria_legal_postventa_datos_personales_entity>, IDisposable
    {
        Task<List<validacion_rectificatoria_legal_postventa_datos_personales_entity>> GetByExpediente(long id_expediente);
    }
}

