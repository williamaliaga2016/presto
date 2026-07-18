using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirNotariaReparoAbogadosRepository
        : IMultibancaGenericRepository<corregir_notaria_reparo_abogados_entity>, IDisposable
    {
        Task<corregir_notaria_reparo_abogados_entity?> GetByExpediente(long id_expediente);
    }
}
