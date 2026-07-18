using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionCreditoApplication : IMultibancaGenericApplication<revisar_datos_operacion_credito>
    {
        Task<revisar_datos_operacion_credito> GetByExpediente(long id_expediente);

        Task<revisar_datos_operacion_credito> GetByExpediente(long id_expediente, int id_revisar_datos_operacion);
    }
}
