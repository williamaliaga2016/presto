using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteVendedorApplication :
        IMultibancaGenericApplication<carga_operacion_banco_antecedente_vendedor>
    {
        Task<List<carga_operacion_banco_antecedente_vendedor>> GetByExpediente(long id_expediente);
    }
}
