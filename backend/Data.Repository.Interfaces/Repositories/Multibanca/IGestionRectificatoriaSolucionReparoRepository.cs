using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGestionRectificatoriaSolucionReparoRepository : IMultibancaGenericRepository<gestion_rectificatoria_solucion_reparo_entity>,IDisposable
    {
        Task<gestion_rectificatoria_solucion_reparo_entity?> GetByExpediente(long id_expediente);
    }
}
