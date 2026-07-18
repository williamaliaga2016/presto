using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RectificatoriaFirmaDetalleRepository
        : MultibancaGenericRepository<rectificatoria_firma_detalle_entity>,
          IRectificatoriaFirmaDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RectificatoriaFirmaDetalleRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

    }
}
