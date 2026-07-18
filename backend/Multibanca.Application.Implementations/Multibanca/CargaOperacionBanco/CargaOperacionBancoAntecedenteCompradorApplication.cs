using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoAntecedenteCompradorApplication :
        MultibancaGenericApplication<
            carga_operacion_banco_antecedente_comprador,
            carga_operacion_banco_antecedente_comprador_entity,
            ICargaOperacionBancoAntecedenteCompradorRepository>,
        ICargaOperacionBancoAntecedenteCompradorApplication
    {
        private readonly ICargaOperacionBancoAntecedenteCompradorRepository CargaOperacionBancoAntecedenteCompradorRepositoryProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoAntecedenteCompradorApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoAntecedenteCompradorRepository _cargaOperacionBancoAntecedenteCompradorRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _cargaOperacionBancoAntecedenteCompradorRepository, _mapper)
        {
            CargaOperacionBancoAntecedenteCompradorRepositoryProvider = _cargaOperacionBancoAntecedenteCompradorRepository;
            Mapper = _mapper;
        }

        public async Task<List<carga_operacion_banco_antecedente_comprador>> GetByExpediente(long id_expediente)
        {
            List<carga_operacion_banco_antecedente_comprador_entity> entity =
                await CargaOperacionBancoAntecedenteCompradorRepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<List<carga_operacion_banco_antecedente_comprador>>(entity);
        }
    }
}
