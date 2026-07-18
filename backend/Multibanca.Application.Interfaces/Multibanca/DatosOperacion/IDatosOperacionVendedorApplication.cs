using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionVendedorApplication : IMultibancaGenericApplication<datos_operacion_vendedor>
    {
        Task<List<datos_operacion_vendedor>> GetByExpediente(long id_expediente);
    }
}
