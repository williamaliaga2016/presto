using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteVendedorRepository : IMultibancaGenericRepository<carga_operacion_banco_antecedente_vendedor_entity>, IDisposable
    {
        Task<List<carga_operacion_banco_antecedente_vendedor_entity>> GetByExpediente(long id_expediente);
    }
}
