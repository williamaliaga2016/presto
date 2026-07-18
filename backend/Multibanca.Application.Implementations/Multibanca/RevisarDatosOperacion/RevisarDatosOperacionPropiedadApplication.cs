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
    public class RevisarDatosOperacionPropiedadApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion_propiedad,
            revisar_datos_operacion_propiedad_entity,
            IRevisarDatosOperacionPropiedadRepository>,
        IRevisarDatosOperacionPropiedadApplication
    {
        private readonly IRevisarDatosOperacionPropiedadRepository RepositoryProvider;
        private readonly IMapper Mapper;
        private readonly IDatosOperacionPropiedadApplication DatosOperacionPropiedadApplicationProvider;

        public RevisarDatosOperacionPropiedadApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarDatosOperacionPropiedadRepository _repository,
            IMapper _mapper,
            IDatosOperacionPropiedadApplication _datosOperacionPropiedadApplication)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
            DatosOperacionPropiedadApplicationProvider = _datosOperacionPropiedadApplication;
        }

        public async Task<revisar_datos_operacion_propiedad> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            var domain = Mapper.Map<revisar_datos_operacion_propiedad>(entity);

            if (domain.id_revisar_datos_operacion_propiedad == 0)
            {
                await CompletarDatosHeredados(domain, id_expediente);
            }

            return domain;
        }

        public async Task<revisar_datos_operacion_propiedad> GetByExpediente(long id_expediente, int id_revisar_datos_operacion)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente, id_revisar_datos_operacion);
            var domain = Mapper.Map<revisar_datos_operacion_propiedad>(entity);

            if (domain.id_revisar_datos_operacion_propiedad == 0)
            {
                await CompletarDatosHeredados(domain, id_expediente);
            }

            return domain;
        }

        private async Task CompletarDatosHeredados(
            revisar_datos_operacion_propiedad domain,
            long id_expediente)
        {
            var source = await DatosOperacionPropiedadApplicationProvider.GetByExpediente(id_expediente);

            if (source == null)
            {
                return;
            }

            domain.id_expediente = id_expediente;
            domain.tipo_propiedad = source.tipo_propiedad;
            domain.estado = source.estado;
            domain.tipo_venta = source.tipo_venta;
            domain.tipo_construccion = source.tipo_construccion;
            domain.tipo_direccion = source.tipo_direccion;
            domain.direccion = source.direccion;
            domain.villa_condominio = source.villa_condominio;
            domain.numero = source.numero;
            domain.numero_casa_habitantes = source.numero_casa_habitantes;
            domain.conjunto = source.conjunto;
            domain.manzana = source.manzana;
            domain.lote = source.lote;
            domain.region = source.region;
            domain.comuna = source.comuna;
            domain.existe_rol_avaluo = source.existe_rol_avaluo;
            domain.rol_avaluo_1 = source.rol_avaluo_1;
            domain.rol_avaluo_2 = source.rol_avaluo_2;
            domain.valor_avaluo_pesos = source.valor_avaluo_pesos;
            domain.enviar_reparo = null;
            domain.observaciones = null;
        }
    }
}
