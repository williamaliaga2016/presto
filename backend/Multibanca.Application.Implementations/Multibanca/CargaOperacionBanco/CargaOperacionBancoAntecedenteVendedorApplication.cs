using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoAntecedenteVendedorApplication :
        MultibancaGenericApplication<
            carga_operacion_banco_antecedente_vendedor,
            carga_operacion_banco_antecedente_vendedor_entity,
            ICargaOperacionBancoAntecedenteVendedorRepository>,
        ICargaOperacionBancoAntecedenteVendedorApplication
    {
        private readonly ICargaOperacionBancoAntecedenteVendedorRepository CargaOperacionBancoAntecedenteVendedorRepositoryProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoAntecedenteVendedorApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoAntecedenteVendedorRepository _cargaOperacionBancoAntecedenteVendedorRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _cargaOperacionBancoAntecedenteVendedorRepository, _mapper)
        {
            CargaOperacionBancoAntecedenteVendedorRepositoryProvider = _cargaOperacionBancoAntecedenteVendedorRepository;
            Mapper = _mapper;
        }

        public async Task<List<carga_operacion_banco_antecedente_vendedor>> GetByExpediente(long id_expediente)
        {
            List<carga_operacion_banco_antecedente_vendedor_entity> entity =
                await CargaOperacionBancoAntecedenteVendedorRepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<List<carga_operacion_banco_antecedente_vendedor>>(entity);
        }
    }
}
