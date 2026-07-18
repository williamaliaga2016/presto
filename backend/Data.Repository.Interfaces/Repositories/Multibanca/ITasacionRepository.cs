using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ITasacionRepository : IMultibancaGenericRepository<tasacion_entity>, IDisposable
    {
        Task<tasacion_entity?> GetByExpediente(long id_expediente);
    }
}
