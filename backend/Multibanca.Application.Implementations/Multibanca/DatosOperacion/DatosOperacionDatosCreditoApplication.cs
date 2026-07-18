using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class DatosOperacionDatosCreditoApplication :
        MultibancaGenericApplication<
            datos_operacion_datos_credito,
            datos_operacion_datos_credito_entity,
            IDatosOperacionDatosCreditoRepository>,
        IDatosOperacionDatosCreditoApplication
    {
        private readonly IDatosOperacionDatosCreditoRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public DatosOperacionDatosCreditoApplication(
            MultibancaDBContext _multibancaDBContext,
            IDatosOperacionDatosCreditoRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<datos_operacion_datos_credito> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<datos_operacion_datos_credito>(entity);
        }
    }
}
