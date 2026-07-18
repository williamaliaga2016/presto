using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoAntecedenteCompradorRepository : IMultibancaGenericRepository<carga_operacion_banco_antecedente_comprador_entity>, IDisposable
    {
        Task<List<carga_operacion_banco_antecedente_comprador_entity>> GetByExpediente(long id_expediente);
    }
}
