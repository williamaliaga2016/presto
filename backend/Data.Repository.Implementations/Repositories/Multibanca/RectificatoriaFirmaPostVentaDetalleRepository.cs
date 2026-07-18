using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RectificatoriaFirmaPostVentaDetalleRepository
        : MultibancaGenericRepository<rectificatoria_firma_post_venta_detalle_entity>,
          IRectificatoriaFirmaPostVentaDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RectificatoriaFirmaPostVentaDetalleRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

    }
}
