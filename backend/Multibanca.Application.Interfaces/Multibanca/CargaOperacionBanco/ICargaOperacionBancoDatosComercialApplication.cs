using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoDatosComercialApplication :
        IMultibancaGenericApplication<carga_operacion_banco_datos_comercial>
    {
        Task<carga_operacion_banco_datos_comercial> GetByExpediente(long id_expediente);
    }
}
