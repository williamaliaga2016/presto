using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IEntregarCarpetaRepository : IMultibancaGenericRepository<entregar_carpeta_entity>, IDisposable
    {
        Task<entregar_carpeta_entity?> GetByExpediente(long id_expediente);
    }
}

