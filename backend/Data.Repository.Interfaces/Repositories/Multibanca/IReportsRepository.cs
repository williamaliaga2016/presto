using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReportsRepository : IMultibancaGenericRepository<reports_entity>, IDisposable
    {
    }
}