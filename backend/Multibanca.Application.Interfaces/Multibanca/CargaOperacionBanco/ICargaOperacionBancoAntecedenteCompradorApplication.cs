using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteCompradorApplication :
        IMultibancaGenericApplication<carga_operacion_banco_antecedente_comprador>
    {
        Task<List<carga_operacion_banco_antecedente_comprador>> GetByExpediente(long id_expediente);
    }
}
