using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionCompradorApplication : IMultibancaGenericApplication<revisar_datos_operacion_comprador>
    {
        Task<IList<revisar_datos_operacion_comprador>> GetListByExpediente(long id_expediente);
    }
}
