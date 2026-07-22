using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class FirmarEscrituraClienteRepository : MultibancaGenericRepository<firmar_escritura_cliente_entity>, IFirmarEscrituraClienteRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public FirmarEscrituraClienteRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<firmar_escritura_cliente_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.firmar_escritura_cliente
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }

        public async Task<object?> GetDatosNotariaHerencia(long idExpediente)
        {
            // TODO: Implementar consulta real cuando la tabla validar_cumplimiento_politicas esté lista
            // Revisar HU BBV-86 (CA-07) — fallback para datos de notaría heredados
            await Task.CompletedTask;

            return new
            {
                notaria = (string?)null,
                fecha_notaria = (DateTime?)null,
                numero_notaria = (int?)null,
                ciudad_notaria = (string?)null
            };
        }
    }
}
