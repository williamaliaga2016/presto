using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRevisarIngresoDatosCreditoApplication : IMultibancaGenericApplication<revisar_ingreso_datos_credito>
    {
        Task<revisar_ingreso_datos_credito> GetByExpediente(long id_expediente);
    }
}
