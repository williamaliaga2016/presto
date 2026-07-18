using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class RevisarIngresoDatosCreditoApplication :
        MultibancaGenericApplication<
            revisar_ingreso_datos_credito,
            revisar_ingreso_datos_credito_entity,
            IRevisarIngresoDatosCreditoRepository>,
        IRevisarIngresoDatosCreditoApplication
    {
        private readonly IRevisarIngresoDatosCreditoRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public RevisarIngresoDatosCreditoApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarIngresoDatosCreditoRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<revisar_ingreso_datos_credito> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<revisar_ingreso_datos_credito>(entity);
        }
    }
}
