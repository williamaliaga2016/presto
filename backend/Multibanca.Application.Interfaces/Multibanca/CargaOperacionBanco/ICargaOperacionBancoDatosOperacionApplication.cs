using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoDatosOperacionApplication :
        IMultibancaGenericApplication<carga_operacion_banco_datos_operacion>
    {
        Task<carga_operacion_banco_datos_operacion> GetByExpediente(long id_expediente);
    }
}