using Common.Application.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionBancoApplication: IMultibancaGenericApplication<revisar_datos_operacion_banco>
    {
        Task<revisar_datos_operacion_banco> GetByExpediente(long id_expediente);
    }
}
