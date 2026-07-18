using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionCompradorRepository :
        MultibancaGenericRepository<revisar_datos_operacion_comprador_entity>,
        IRevisarDatosOperacionCompradorRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarDatosOperacionCompradorRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<IList<revisar_datos_operacion_comprador_entity>> GetListByExpediente(long id_expediente)
        {
            return await MultibancaDBContext.Set<revisar_datos_operacion_comprador_entity>()
                .Where(q => q.id_expediente == id_expediente && q.is_active && q.row_status)
                .OrderBy(q => q.id_revisar_datos_operacion_comprador)
                .ToListAsync();
        }
    }
}
