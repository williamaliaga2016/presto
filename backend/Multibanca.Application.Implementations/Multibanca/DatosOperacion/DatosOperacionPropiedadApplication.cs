using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class DatosOperacionPropiedadApplication :
        MultibancaGenericApplication<
            datos_operacion_propiedad,
            datos_operacion_propiedad_entity,
            IDatosOperacionPropiedadRepository>,
        IDatosOperacionPropiedadApplication
    {
        private readonly IDatosOperacionPropiedadRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public DatosOperacionPropiedadApplication(
            MultibancaDBContext _multibancaDBContext,
            IDatosOperacionPropiedadRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<datos_operacion_propiedad> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<datos_operacion_propiedad>(entity);
        }

        public async Task<List<datos_operacion_propiedad>> GetAllByExpediente(long id_expediente)
        {
            var entities = await RepositoryProvider.GetAllByExpediente(id_expediente);
            return Mapper.Map<List<datos_operacion_propiedad>>(entities);
        }
    }
}
