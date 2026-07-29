using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class VoboGerenciaCohRepository
        : MultibancaGenericRepository<vobo_gerencia_coh_entity>, IVoboGerenciaCohRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public VoboGerenciaCohRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<vobo_gerencia_coh_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.vobo_gerencia_coh
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }

        public async Task<VoboGerenciaCohHerencia?> GetDatosHerencia(long idExpediente)
        {
            // Fuente principal: Excepcion Desembolso (actividad previa a esta).
            // Si no hay registro activo, no hay Datos_Heredados que mostrar (Req 2.4).
            var excepcion = await MultibancaDBContext.excepcion_desembolso
                .AsNoTracking()
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            if (excepcion == null)
                return null;

            // Notaría: capturada originalmente en Firmar Escritura Cliente (BBV-86).
            var firmaCliente = await MultibancaDBContext.firmar_escritura_cliente
                .AsNoTracking()
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            return new VoboGerenciaCohHerencia
            {
                nombre_notaria          = firmaCliente?.notaria,
                ciudad_notaria          = firmaCliente?.ciudad_notaria,
                excepcion_autorizada    = excepcion.excepcion_autorizada,
                requiere_vobo_gerencia  = excepcion.requiere_vobo_gerencia,
                confirmacion_excepcion  = excepcion.confirmacion_excepcion,
                observaciones_excepcion = excepcion.observaciones_excepcion
            };
        }
    }
}
