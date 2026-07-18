using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReparoFormularioRepository
        : IMultibancaGenericRepository<reparo_formulario_entity>, IDisposable
    {
        Task<reparo_formulario_entity?> GetByExpediente(long id_expediente);
    }
}