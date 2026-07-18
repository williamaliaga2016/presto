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
    public class RevisarDatosOperacionCreditoApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion_credito,
            revisar_datos_operacion_credito_entity,
            IRevisarDatosOperacionCreditoRepository>,
        IRevisarDatosOperacionCreditoApplication
    {
        private readonly IRevisarDatosOperacionCreditoRepository RepositoryProvider;
        private readonly IMapper Mapper;
        private readonly IDatosOperacionDatosCreditoApplication DatosOperacionDatosCreditoApplicationProvider;

        public RevisarDatosOperacionCreditoApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarDatosOperacionCreditoRepository _repository,
            IMapper _mapper,
            IDatosOperacionDatosCreditoApplication _datosOperacionDatosCreditoApplication)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
            DatosOperacionDatosCreditoApplicationProvider = _datosOperacionDatosCreditoApplication;
        }

        public async Task<revisar_datos_operacion_credito> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            var domain = Mapper.Map<revisar_datos_operacion_credito>(entity);

            if (domain.id_revisar_datos_operacion_credito == 0)
            {
                await CompletarDatosHeredados(domain, id_expediente);
            }

            return domain;
        }

        public async Task<revisar_datos_operacion_credito> GetByExpediente(long id_expediente, int id_revisar_datos_operacion)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente, id_revisar_datos_operacion);
            var domain = Mapper.Map<revisar_datos_operacion_credito>(entity);

            if (domain.id_revisar_datos_operacion_credito == 0)
            {
                await CompletarDatosHeredados(domain, id_expediente);
            }

            return domain;
        }

        private async Task CompletarDatosHeredados(
            revisar_datos_operacion_credito domain,
            long id_expediente)
        {
            var source = await DatosOperacionDatosCreditoApplicationProvider.GetByExpediente(id_expediente);

            if (source == null)
            {
                return;
            }

            domain.id_expediente = id_expediente;

            domain.santiago = source.ubicacion == true ? true : null;
            domain.regiones = source.ubicacion == false ? true : null;

            domain.tipo_operacion = source.tipo_operacion;
            domain.fines_generales = source.fines_generales;
            domain.nombre_proyecto = source.nombre_proyecto;
            domain.credito_segunda_vivienda = source.credito_segunda_vivienda;
            domain.inmobiliaria = source.inmobiliaria;
            domain.vivienda_social = source.vivienda_social;
            domain.dfl2 = source.dfl2;
            domain.propietario_0_1_dfl2 = source.propietario_dfl2;           // renamed
            domain.recepcion_final_mayor_2 = source.recepcion_final_mayor_2_anios; // renamed
            domain.porcentaje_impuesto = source.porcentaje_impuesto;
            domain.monto_credito_afecto = source.monto_credito_afecto_impuesto; // renamed
            domain.impuesto_a_pagar = source.impuesto_a_pagar;
            domain.observaciones = null;
        }
    }
}
