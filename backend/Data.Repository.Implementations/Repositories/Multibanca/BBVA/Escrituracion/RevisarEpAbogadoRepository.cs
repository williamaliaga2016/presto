using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RevisarEpAbogadoRepository : MultibancaGenericRepository<revisar_ep_abogado_entity>, IRevisarEpAbogadoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarEpAbogadoRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<revisar_ep_abogado_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.revisar_ep_abogado
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }

        public async Task<firmar_escritura_cliente_entity?> GetDatosHerencia(long idExpediente)
        {
            return await MultibancaDBContext.firmar_escritura_cliente
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene el estado de la carta de aprobación del expediente.
        /// Retorna el campo 'estado' de carta_aprobacion_bbva (PENDIENTE | GENERADO | ERROR).
        /// TODO: La lógica de vigencia (Vigente/Por_Vencer/Vencido) se definirá cuando
        /// negocio confirme de dónde sale y cómo se calcula.
        /// TODO: Según la historia BBV-130 CA09 - la crta de aprobación es importante preguntar
        /// </summary>
        public async Task<string?> GetEstadoCartaAprobacion(long idExpediente)
        {
            var carta = await MultibancaDBContext.CartaAprobacionBbva
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            return carta?.estado;
        }
    }
}
