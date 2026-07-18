using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ITasacionDetalleApplication : IMultibancaGenericApplication<tasacion_detalle>
    {
        Task<List<tasacion_detalle>> GetByExpediente(long id_expediente);
        Task<List<tasacion_detalle>> GetByTasacion(int id_tasacion);
    }
}
