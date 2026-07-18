using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoDatosOperacionApplication :
        MultibancaGenericApplication<
            carga_operacion_banco_datos_operacion,
            carga_operacion_banco_datos_operacion_entity,
            ICargaOperacionBancoDatosOperacionRepository>,
        ICargaOperacionBancoDatosOperacionApplication
    {
        private readonly ICargaOperacionBancoDatosOperacionRepository CargaOperacionBancoDatosOperacionRepositoryProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoDatosOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoDatosOperacionRepository _cargaOperacionBancoDatosOperacionRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _cargaOperacionBancoDatosOperacionRepository, _mapper)
        {
            CargaOperacionBancoDatosOperacionRepositoryProvider = _cargaOperacionBancoDatosOperacionRepository;
            Mapper = _mapper;
        }

        public async Task<carga_operacion_banco_datos_operacion> GetByExpediente(long id_expediente)
        {
            carga_operacion_banco_datos_operacion_entity entity =
                await CargaOperacionBancoDatosOperacionRepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<carga_operacion_banco_datos_operacion>(entity);
        }
    }
}