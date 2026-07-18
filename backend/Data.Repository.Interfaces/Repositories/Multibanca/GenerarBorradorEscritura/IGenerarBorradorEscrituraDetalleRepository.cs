using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura
{
    public interface IGenerarBorradorEscrituraDetalleRepository : IMultibancaGenericRepository<generar_borrador_escritura_detalle_entity>, IDisposable
    {
        Task<List<generar_borrador_escritura_detalle_entity>> GetList(int id_generar_borrador_escritura,long id_expediente);
    }
}
