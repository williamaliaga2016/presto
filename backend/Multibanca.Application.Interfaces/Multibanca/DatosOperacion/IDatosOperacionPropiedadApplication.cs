using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionPropiedadApplication : IMultibancaGenericApplication<datos_operacion_propiedad>
    {
        Task<datos_operacion_propiedad> GetByExpediente(long id_expediente);
        Task<List<datos_operacion_propiedad>> GetAllByExpediente(long id_expediente);
    }
}
