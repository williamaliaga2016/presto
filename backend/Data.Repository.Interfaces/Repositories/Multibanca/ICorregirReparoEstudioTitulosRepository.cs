using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoEstudioTitulosRepository
        : IMultibancaGenericRepository<corregir_reparo_estudio_titulos_entity>, IDisposable
    {
        Task<corregir_reparo_estudio_titulos_entity?> GetByExpediente(long id_expediente);
    }
}
