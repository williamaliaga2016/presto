using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionDatosCreditoApplication : IMultibancaGenericApplication<datos_operacion_datos_credito>
    {
        Task<datos_operacion_datos_credito> GetByExpediente(long id_expediente);
    }
}
