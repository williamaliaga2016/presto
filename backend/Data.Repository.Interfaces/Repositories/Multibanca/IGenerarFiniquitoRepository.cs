using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarFiniquitoRepository : IMultibancaGenericRepository<generar_finiquito_entity>, IDisposable
    {
        Task<generar_finiquito_entity?> GetByExpediente(long id_expediente);
    }
}
