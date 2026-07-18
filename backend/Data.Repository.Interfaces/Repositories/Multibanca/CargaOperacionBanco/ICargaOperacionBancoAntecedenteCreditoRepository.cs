using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteCreditoRepository : IMultibancaGenericRepository<carga_operacion_banco_antecedente_credito_entity>, IDisposable
    {
        Task<carga_operacion_banco_antecedente_credito_entity> GetByExpediente(long id_expediente);
    }
}
