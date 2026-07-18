using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class DatosOperacionCompradorApplication :
        MultibancaGenericApplication<
            datos_operacion_comprador,
            datos_operacion_comprador_entity,
            IDatosOperacionCompradorRepository>,
        IDatosOperacionCompradorApplication
    {
        private readonly IDatosOperacionCompradorRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public DatosOperacionCompradorApplication(
            MultibancaDBContext _multibancaDBContext,
            IDatosOperacionCompradorRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<List<datos_operacion_comprador>> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<List<datos_operacion_comprador>>(entity);
        }
    }
}
