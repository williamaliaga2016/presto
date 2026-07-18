using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionBancoApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion_banco,
            revisar_datos_operacion_banco_entity,
            IRevisarDatosOperacionBancoRepository>,
        IRevisarDatosOperacionBancoApplication
    {
        private readonly IRevisarDatosOperacionBancoRepository RevisarDatosOperacionBancoRepositoryProvider;
        private readonly IMapper Mapper;
        private readonly IDatosOperacionBancoAcreedorApplication DatosOperacionBancoAcreedorApplicationProvider;

        public RevisarDatosOperacionBancoApplication(
            MultibancaDBContext multibancaDBContext,
            IRevisarDatosOperacionBancoRepository revisarDatosOperacionBancoRepository,
            IMapper mapper,
            IDatosOperacionBancoAcreedorApplication datosOperacionBancoAcreedorApplication)
            : base(multibancaDBContext, revisarDatosOperacionBancoRepository, mapper)
        {
            RevisarDatosOperacionBancoRepositoryProvider = revisarDatosOperacionBancoRepository;
            Mapper = mapper;
            DatosOperacionBancoAcreedorApplicationProvider = datosOperacionBancoAcreedorApplication;
        }

        public async Task<revisar_datos_operacion_banco> GetByExpediente(long id_expediente)
        {
            var entity = await RevisarDatosOperacionBancoRepositoryProvider.GetByExpediente(id_expediente);
            var domain = Mapper.Map<revisar_datos_operacion_banco>(entity);

            if (domain.id_revisar_datos_operacion_banco == 0)
            {
                await CompletarDatosHeredados(domain, id_expediente);
            }

            return domain;
        }

        private async Task CompletarDatosHeredados(
            revisar_datos_operacion_banco domain,
            long id_expediente)
        {
            var source = await DatosOperacionBancoAcreedorApplicationProvider.GetByExpediente(id_expediente);

            if (source == null)
            {
                return;
            }

            domain.id_expediente = id_expediente;
            domain.cuenta_carta_resguardo = source.cuenta_carta_resguardo;
            domain.institucion = source.institucion;
            domain.rut_banco_acreedor = source.rut_banco_acreedor;
            domain.enviar_a_reparo = null;
            domain.observaciones = null;
        }
    }
}
