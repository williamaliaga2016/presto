using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoEntregarCarpetaRepository
        : IMultibancaGenericRepository<corregir_reparo_entregar_carpeta_entity>, IDisposable
    {
        Task<corregir_reparo_entregar_carpeta_entity?> GetByExpediente(long id_expediente);
    }
}
