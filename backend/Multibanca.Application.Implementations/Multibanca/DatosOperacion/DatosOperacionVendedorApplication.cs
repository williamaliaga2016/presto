using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class DatosOperacionVendedorApplication :
        MultibancaGenericApplication<
            datos_operacion_vendedor,
            datos_operacion_vendedor_entity,
            IDatosOperacionVendedorRepository>,
        IDatosOperacionVendedorApplication
    {
        private readonly IDatosOperacionVendedorRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public DatosOperacionVendedorApplication(
            MultibancaDBContext _multibancaDBContext,
            IDatosOperacionVendedorRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<List<datos_operacion_vendedor>> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<List<datos_operacion_vendedor>>(entity);
        }
    }
}
