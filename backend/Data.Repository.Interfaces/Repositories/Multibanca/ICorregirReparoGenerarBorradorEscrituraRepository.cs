using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoGenerarBorradorEscrituraRepository
        : IMultibancaGenericRepository<corregir_reparo_generar_borrador_escritura_entity>, IDisposable
    {
        Task<corregir_reparo_generar_borrador_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
