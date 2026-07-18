using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGestionRectificatoriaEscrituraFirmadaPostventaRepository : IMultibancaGenericRepository<gestion_rectificatoria_escritura_firmada_postventa_entity>, IDisposable
    {
        Task<gestion_rectificatoria_escritura_firmada_postventa_entity?> GetByExpediente(long id_expediente);

    }
}
