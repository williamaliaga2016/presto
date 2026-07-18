using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGestionRectificatoriaRepository : IMultibancaGenericRepository<gestion_rectificatoria_entity>,IDisposable
    {
        Task<gestion_rectificatoria_entity?> GetByExpediente(long id_expediente);
    }
}
