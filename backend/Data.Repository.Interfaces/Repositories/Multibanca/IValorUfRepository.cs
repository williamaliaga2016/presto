using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IValorUfRepository : IMultibancaGenericRepository<valor_uf_entity>, IDisposable
    {
        Task<valor_uf_entity?> GetByFecha(DateTime fecha);
        Task<valor_uf_entity?> GetMasReciente();
    }
}
