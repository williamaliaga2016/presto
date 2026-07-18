using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionFiadorGaranteApplication : IMultibancaGenericApplication<datos_operacion_fiador_garante>
    {
        Task<List<datos_operacion_fiador_garante>> GetByExpediente(long id_expediente);
    }
}
