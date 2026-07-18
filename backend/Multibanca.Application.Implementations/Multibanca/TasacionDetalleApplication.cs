using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class TasacionDetalleApplication :
        MultibancaGenericApplication<tasacion_detalle, tasacion_detalle_entity, ITasacionDetalleRepository>,
        ITasacionDetalleApplication
    {
        private readonly ITasacionDetalleRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public TasacionDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            ITasacionDetalleRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<List<tasacion_detalle>> GetByExpediente(long id_expediente)
        {
            var entities = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<List<tasacion_detalle>>(entities);
        }

        public async Task<List<tasacion_detalle>> GetByTasacion(int id_tasacion)
        {
            var entities = await RepositoryProvider.GetByTasacion(id_tasacion);
            return Mapper.Map<List<tasacion_detalle>>(entities);
        }
    }
}
