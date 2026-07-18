using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteCreditoApplication :
        IMultibancaGenericApplication<carga_operacion_banco_antecedente_credito>
    {
        Task<carga_operacion_banco_antecedente_credito> GetByExpediente(long id_expediente);
    }
}
