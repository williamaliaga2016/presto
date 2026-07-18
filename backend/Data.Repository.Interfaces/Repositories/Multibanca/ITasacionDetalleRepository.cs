using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ITasacionDetalleRepository : IMultibancaGenericRepository<tasacion_detalle_entity>, IDisposable
    {
        Task<List<tasacion_detalle_entity>> GetByExpediente(long id_expediente);
        Task<List<tasacion_detalle_entity>> GetByTasacion(int id_tasacion);
    }
}
