using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionBancoAcreedorApplication : IMultibancaGenericApplication<datos_operacion_banco_acreedor>
    {
        Task<datos_operacion_banco_acreedor> GetByExpediente(long id_expediente);
    }
}
