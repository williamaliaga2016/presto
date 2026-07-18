using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ValorUfRepository : MultibancaGenericRepository<valor_uf_entity>, IValorUfRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ValorUfRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<valor_uf_entity?> GetByFecha(DateTime fecha)
        {
            DateTime fechaSoloFecha = fecha.Date;
            return await MultibancaDBContext.Set<valor_uf_entity>()
                .AsNoTracking()
                .Where(x => x.fecha == fechaSoloFecha && x.row_status && x.is_active)
                .FirstOrDefaultAsync();
        }

        public async Task<valor_uf_entity?> GetMasReciente()
        {
            return await MultibancaDBContext.Set<valor_uf_entity>()
                .AsNoTracking()
                .Where(x => x.row_status && x.is_active)
                .OrderByDescending(x => x.fecha)
                .FirstOrDefaultAsync();
        }
    }
}
