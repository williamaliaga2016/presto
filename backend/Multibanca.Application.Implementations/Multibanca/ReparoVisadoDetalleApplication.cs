using AutoMapper;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ReparoVisadoDetalleApplication : IReparoVisadoDetalleApplication
    {
        private readonly IReparoVisadoDetalleRepository ReparoVisadoDetalleRepositoryProvider;
        private readonly IMapper Mapper;

        public ReparoVisadoDetalleApplication(
            IReparoVisadoDetalleRepository _reparoVisadoDetalleRepository,
            IMapper _mapper)
        {
            ReparoVisadoDetalleRepositoryProvider = _reparoVisadoDetalleRepository;
            Mapper = _mapper;
        }

        public async Task<List<reparo_visado_detalle>> GetByExpediente(long id_expediente)
        {
            var entities = await ReparoVisadoDetalleRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<List<reparo_visado_detalle>>(entities) ?? new List<reparo_visado_detalle>();
        }

        public async Task MarkSubsanado(int id_visar_operacion, int userId)
        {
            await ReparoVisadoDetalleRepositoryProvider.MarkSubsanado(id_visar_operacion, userId);
        }
    }
}
