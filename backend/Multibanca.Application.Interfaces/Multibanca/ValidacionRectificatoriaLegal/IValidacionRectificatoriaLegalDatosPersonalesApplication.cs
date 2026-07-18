using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;

namespace Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalDatosPersonalesApplication : IMultibancaGenericApplication<validacion_rectificatoria_legal_datos_personales>
    {
        Task<List<validacion_rectificatoria_legal_datos_personales>> GetByExpediente(long id_expediente);
    }
}

