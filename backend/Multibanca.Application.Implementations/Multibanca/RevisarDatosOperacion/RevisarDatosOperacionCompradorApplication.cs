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
    public class RevisarDatosOperacionCompradorApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion_comprador,
            revisar_datos_operacion_comprador_entity,
            IRevisarDatosOperacionCompradorRepository>,
        IRevisarDatosOperacionCompradorApplication
    {
        private readonly IRevisarDatosOperacionCompradorRepository RepositoryProvider;
        private readonly IMapper Mapper;
        private readonly IDatosOperacionCompradorApplication DatosOperacionCompradorApplicationProvider;

        public RevisarDatosOperacionCompradorApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarDatosOperacionCompradorRepository _repository,
            IMapper _mapper,
            IDatosOperacionCompradorApplication _datosOperacionCompradorApplication)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            Mapper = _mapper;
            DatosOperacionCompradorApplicationProvider = _datosOperacionCompradorApplication;
        }

        public async Task<IList<revisar_datos_operacion_comprador>> GetListByExpediente(long id_expediente)
        {
            var entities = await RepositoryProvider.GetListByExpediente(id_expediente);
            var list = Mapper.Map<IList<revisar_datos_operacion_comprador>>(entities);

            if (list == null || list.Count == 0)
            {
                list = await HeredarDesdeDatosOperacion(id_expediente);
            }

            return list;
        }

        private async Task<IList<revisar_datos_operacion_comprador>> HeredarDesdeDatosOperacion(long id_expediente)
        {
            var sourceList = await DatosOperacionCompradorApplicationProvider.GetByExpediente(id_expediente);

            if (sourceList == null || sourceList.Count == 0)
            {
                return new List<revisar_datos_operacion_comprador>();
            }

            return sourceList.Select(src => new revisar_datos_operacion_comprador
            {
                id_revisar_datos_operacion_comprador = 0,
                id_revisar_datos_operacion = 0,
                id_expediente = id_expediente,
                rut = src.rut,
                tipo_persona = src.tipo_persona,
                razon_social = src.razon_social,
                nombres = src.nombres,
                apellido_paterno = src.apellido_paterno,
                apellido_materno = src.apellido_materno,
                fecha_nacimiento = src.fecha_nacimiento,
                genero = src.genero,
                estado_civil = src.estado_civil,
                nacionalidad = src.nacionalidad,
                profesion = src.profesion,
                relacion_titular = src.relacion_titular,
                direccion = src.direccion,
                direccion_env_esc = src.direccion_env_esc,
                region = src.region,
                region_env_esc = src.region_env_esc,
                comuna = src.comuna,
                comuna_env_esc = src.comuna_env_esc,
                direccion_env_div = src.direccion_env_div,
                region_env_div = src.region_env_div,
                comuna_env_div = src.comuna_env_div,
                tipo_dir_dividendo = src.tipo_dir_dividendo,
                telefono = src.telefono,
                telefono_comercial = src.telefono_comercial,
                telefono_movil = src.telefono_movil,
                email = src.email,
                email2 = src.email2,
                observaciones = null,
                is_active = true,
                row_status = true,
            }).ToList();
        }
    }
}
