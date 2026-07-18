using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionVendedorApplication : IMultibancaGenericApplication<revisar_datos_operacion_vendedor>
    {
        Task<IList<revisar_datos_operacion_vendedor>> GetListByExpediente(long id_expediente);
    }
}
