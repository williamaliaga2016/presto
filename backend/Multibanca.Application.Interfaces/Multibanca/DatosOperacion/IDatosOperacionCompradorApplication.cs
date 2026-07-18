using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionCompradorApplication : IMultibancaGenericApplication<datos_operacion_comprador>
    {
        Task<List<datos_operacion_comprador>> GetByExpediente(long id_expediente);
    }
}
