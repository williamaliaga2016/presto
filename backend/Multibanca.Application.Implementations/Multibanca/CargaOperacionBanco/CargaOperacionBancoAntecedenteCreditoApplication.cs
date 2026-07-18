using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoAntecedenteCreditoApplication :
        MultibancaGenericApplication<
            carga_operacion_banco_antecedente_credito,
            carga_operacion_banco_antecedente_credito_entity,
            ICargaOperacionBancoAntecedenteCreditoRepository>,
        ICargaOperacionBancoAntecedenteCreditoApplication
    {
        private readonly ICargaOperacionBancoAntecedenteCreditoRepository RepositoryProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoAntecedenteCreditoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoAntecedenteCreditoRepository _repository,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
        }

        public async Task<carga_operacion_banco_antecedente_credito> GetByExpediente(long id_expediente)
        {
            carga_operacion_banco_antecedente_credito_entity entity =
                await RepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<carga_operacion_banco_antecedente_credito>(entity);
        }
    }
}
