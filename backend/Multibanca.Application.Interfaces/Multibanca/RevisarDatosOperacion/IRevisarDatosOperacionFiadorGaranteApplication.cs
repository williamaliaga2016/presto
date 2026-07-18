using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionFiadorGaranteApplication : IMultibancaGenericApplication<revisar_datos_operacion_fiador_garante>
    {
        Task<List<revisar_datos_operacion_fiador_garante>> GetByExpediente(long id_expediente);
    }
}
