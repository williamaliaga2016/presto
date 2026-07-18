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
    public class RevisarDatosOperacionFiadorGaranteApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion_fiador_garante,
            revisar_datos_operacion_fiador_garante_entity,
            IRevisarDatosOperacionFiadorGaranteRepository>,
        IRevisarDatosOperacionFiadorGaranteApplication
    {
        private readonly IRevisarDatosOperacionFiadorGaranteRepository RevisarDatosOperacionFiadorGaranteRepositoryProvider;
        private readonly IMapper Mapper;
        private readonly IDatosOperacionFiadorGaranteApplication DatosOperacionFiadorGaranteApplicationProvider;

        public RevisarDatosOperacionFiadorGaranteApplication(
            MultibancaDBContext multibancaDBContext,
            IRevisarDatosOperacionFiadorGaranteRepository revisarDatosOperacionFiadorGaranteRepository,
            IMapper mapper,
            IDatosOperacionFiadorGaranteApplication datosOperacionFiadorGaranteApplication)
            : base(multibancaDBContext, revisarDatosOperacionFiadorGaranteRepository, mapper)
        {
            RevisarDatosOperacionFiadorGaranteRepositoryProvider = revisarDatosOperacionFiadorGaranteRepository;
            Mapper = mapper;
            DatosOperacionFiadorGaranteApplicationProvider = datosOperacionFiadorGaranteApplication;
        }

        public async Task<List<revisar_datos_operacion_fiador_garante>> GetByExpediente(long id_expediente)
        {
            var entities = await RevisarDatosOperacionFiadorGaranteRepositoryProvider.GetByExpediente(id_expediente);
            var list = Mapper.Map<List<revisar_datos_operacion_fiador_garante>>(entities);

            if (list == null || list.Count == 0)
            {
                list = await HeredarDesdeDatosOperacion(id_expediente);
            }

            return list;
        }

        private async Task<List<revisar_datos_operacion_fiador_garante>> HeredarDesdeDatosOperacion(long id_expediente)
        {
            var sourceList = await DatosOperacionFiadorGaranteApplicationProvider.GetByExpediente(id_expediente);

            if (sourceList == null || sourceList.Count == 0)
            {
                return new List<revisar_datos_operacion_fiador_garante>();
            }

            return sourceList.Select(src => new revisar_datos_operacion_fiador_garante
            {
                id_revisar_datos_operacion_fiador_garante = 0,
                id_revisar_datos_operacion = 0,
                id_expediente = id_expediente,
                rut = src.rut,
                nombres = src.nombres,
                apellido_paterno = src.apellido_paterno,
                apellido_materno = src.apellido_materno,
                fecha_nacimiento = src.fecha_nacimiento,
                genero = src.genero,
                estado_civil = src.estado_civil,
                nacionalidad = src.nacionalidad,
                profesion = src.profesion,
                direccion = src.direccion,
                region = src.region,
                comuna = src.comuna,
                telefono_fijo = src.telefono_fijo,
                telefono_movil = src.telefono_movil,
                email = src.email,
                relacion_titular = src.relacion_titular,
                observaciones = null,
                is_active = true,
                row_status = true,
            }).ToList();
        }
    }
}
