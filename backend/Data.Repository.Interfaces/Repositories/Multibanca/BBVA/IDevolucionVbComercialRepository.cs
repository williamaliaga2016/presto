using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA
{
    public interface IDevolucionVbComercialRepository
    {
        Task<devolucion_vb_comercial_entity?> GetByExpediente(long id_expediente);
        Task<devolucion_vb_comercial_entity> Guardar(devolucion_vb_comercial_entity actividad, int userId);
    }
}
