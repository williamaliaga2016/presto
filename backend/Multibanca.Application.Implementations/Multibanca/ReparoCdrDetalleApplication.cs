using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ReparoCdrDetalleApplication
        : MultibancaGenericApplication<reparo_cdr_detalle, reparo_cdr_detalle_entity, IReparoCdrDetalleRepository>,
          IReparoCdrDetalleApplication
    {
        private readonly IReparoCdrDetalleRepository ReparoCdrDetalleRepositoryProvider;
        private readonly IMapper Mapper;

        public ReparoCdrDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IReparoCdrDetalleRepository _reparoCdrDetalleRepository,
            IMapper _mapper) : base(_multibancaDBContext, _reparoCdrDetalleRepository, _mapper)
        {
            ReparoCdrDetalleRepositoryProvider = _reparoCdrDetalleRepository;
            Mapper = _mapper;
        }

        public async Task<reparo_cdr_detalle?> GetByExpediente(long id_expediente)
        {
            var entity = await ReparoCdrDetalleRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null || entity.id_reparo_cdr == 0)
                return null;
            return Mapper.Map<reparo_cdr_detalle>(entity);
        }
    }
}
