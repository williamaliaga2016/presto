using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionPropiedadApplication : IMultibancaGenericApplication<revisar_datos_operacion_propiedad>
    {
        Task<revisar_datos_operacion_propiedad> GetByExpediente(long id_expediente);

        Task<revisar_datos_operacion_propiedad> GetByExpediente(long id_expediente, int id_revisar_datos_operacion);
    }
}
