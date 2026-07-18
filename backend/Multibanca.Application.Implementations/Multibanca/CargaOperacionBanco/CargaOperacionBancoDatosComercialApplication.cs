using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoDatosComercialApplication :
        MultibancaGenericApplication<
            carga_operacion_banco_datos_comercial,
            carga_operacion_banco_datos_comercial_entity,
            ICargaOperacionBancoDatosComercialRepository>,
        ICargaOperacionBancoDatosComercialApplication
    {
        private readonly ICargaOperacionBancoDatosComercialRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoDatosComercialApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoDatosComercialRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<carga_operacion_banco_datos_comercial> GetByExpediente(long id_expediente)
        {
            carga_operacion_banco_datos_comercial_entity entity =
                await RepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<carga_operacion_banco_datos_comercial>(entity);
        }
    }
}
