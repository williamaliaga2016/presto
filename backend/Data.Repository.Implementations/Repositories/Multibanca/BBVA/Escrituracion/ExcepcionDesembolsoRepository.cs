using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class ExcepcionDesembolsoRepository : MultibancaGenericRepository<excepcion_desembolso_entity>, IExcepcionDesembolsoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ExcepcionDesembolsoRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<excepcion_desembolso_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.excepcion_desembolso
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }

        public async Task<OrigenCasoEnum?> DeterminarOrigenCaso(long idExpediente)
        {
            var recepcionBoleta = await MultibancaDBContext.realizar_recepcion_boleta
                .AsNoTracking()
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            if (recepcionBoleta != null)
                return OrigenCasoEnum.RECEPCION_BOLETA;

            var firmarRepLegal = await MultibancaDBContext.firmar_rep_legal
                .AsNoTracking()
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            if (firmarRepLegal != null)
                return OrigenCasoEnum.FIRMAR_REP_LEGAL;

            return null;
        }

        public async Task<ExcepcionDesembolsoHerencia?> GetDatosHerencia(long idExpediente)
        {
            var origen = await DeterminarOrigenCaso(idExpediente);

            if (origen == null)
                return null;

            var firmarRepLegal = await MultibancaDBContext.firmar_rep_legal
                .AsNoTracking()
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();

            if (origen == OrigenCasoEnum.RECEPCION_BOLETA)
            {
                var recepcionBoleta = await MultibancaDBContext.realizar_recepcion_boleta
                    .AsNoTracking()
                    .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                    .OrderByDescending(q => q.id)
                    .FirstOrDefaultAsync();

                return new ExcepcionDesembolsoHerencia
                {
                    conceptoFirma   = firmarRepLegal?.concepto_firma,
                    fechaBoleta     = recepcionBoleta?.fecha_boleta,
                    numeroBoleta    = recepcionBoleta?.numero_boleta,
                    tipoBoleta      = recepcionBoleta?.tipo_boleta,
                    oficinaRegistro = recepcionBoleta?.oficina_registro
                };
            }

            // FIRMAR_REP_LEGAL
            return new ExcepcionDesembolsoHerencia
            {
                conceptoFirma = firmarRepLegal?.concepto_firma
            };
        }
    }
}
