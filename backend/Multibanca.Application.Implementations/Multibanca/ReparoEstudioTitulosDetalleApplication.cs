using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ReparoEstudioTitulosDetalleApplication
        : MultibancaGenericApplication<reparo_estudio_titulos_detalle, reparo_estudio_titulos_detalle_entity, IReparoEstudioTitulosDetalleRepository>,
          IReparoEstudioTitulosDetalleApplication
    {
        private readonly IReparoEstudioTitulosDetalleRepository ReparoEstudioTitulosDetalleRepositoryProvider;
        private readonly IMapper Mapper;

        public ReparoEstudioTitulosDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IReparoEstudioTitulosDetalleRepository _reparoEstudioTitulosDetalleRepository,
            IMapper _mapper) : base(_multibancaDBContext, _reparoEstudioTitulosDetalleRepository, _mapper)
        {
            ReparoEstudioTitulosDetalleRepositoryProvider = _reparoEstudioTitulosDetalleRepository;
            Mapper = _mapper;
        }

        public async Task<List<reparo_estudio_titulos_detalle>> GetByExpediente(long id_expediente)
        {
            var entities = await ReparoEstudioTitulosDetalleRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<List<reparo_estudio_titulos_detalle>>(entities);
        }
    }
}
