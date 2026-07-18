using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoGenerarMemoEscrituraRepository
        : IMultibancaGenericRepository<corregir_reparo_generar_memo_escritura_entity>, IDisposable
    {
        Task<corregir_reparo_generar_memo_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
