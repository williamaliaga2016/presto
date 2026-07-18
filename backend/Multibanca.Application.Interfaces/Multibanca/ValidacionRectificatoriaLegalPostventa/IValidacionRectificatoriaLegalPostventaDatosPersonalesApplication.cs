using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalPostventaDatosPersonalesApplication : IMultibancaGenericApplication<validacion_rectificatoria_legal_postventa_datos_personales>
    {
        Task<List<validacion_rectificatoria_legal_postventa_datos_personales>> GetByExpediente(long id_expediente);
    }
}

