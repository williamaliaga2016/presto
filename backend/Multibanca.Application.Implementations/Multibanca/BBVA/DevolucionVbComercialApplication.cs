using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA.Multibanca.Application.DTOs;

namespace Multibanca.Application.Implementations.Multibanca.BBVA
{
    public class DevolucionVbComercialApplication : IDevolucionVbComercialApplication
    {
        private readonly IDevolucionVbComercialRepository DevolucionVbComercialRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IEncabezadoApplication EncabezadoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IMapper Mapper;
        private readonly MultibancaDBContext _dbContext;

        public DevolucionVbComercialApplication(
            MultibancaDBContext _multibancaDBContext,
            IDevolucionVbComercialRepository repository,
            IWorkflowApplication workflow,
            ICommonApplication commonApplication,
            IEncabezadoApplication encabezadoApplication,
            IBitacoraApplication bitacoraApplication,
            IMapper mapper
        )
        {
            _dbContext = _multibancaDBContext;
            DevolucionVbComercialRepositoryProvider = repository;
            WorkflowApplicationProvider = workflow;
            CommonApplicationProvider = commonApplication;
            EncabezadoApplicationProvider = encabezadoApplication;
            BitacoraApplicacionProvider = bitacoraApplication;
            Mapper = mapper;
        }

        public async Task<DevolucionVbComercialDTO> GetByExpedienteConEncabezado(long id_expediente)
        {
            var dto = new DevolucionVbComercialDTO();

            var devolucionDomain = await GetByExpediente(id_expediente) ?? new devolucion_vb_comercial();

            dto.formulario = new DevolucionVbComerciaFormularioDTO
            {
                id = devolucionDomain.id,
                id_expediente = devolucionDomain.id_expediente,
                id_actividad = devolucionDomain.id_actividad,
                cliente_desiste = devolucionDomain.cliente_desiste,
                motivo_cierre = devolucionDomain.motivo_cierre,
                observaciones = devolucionDomain.observaciones,
            };

            var encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(id_expediente, "ACT_DEVOLUCION_VB_COMERCIAL");

            dto.encabezado = new DevolucionVbComercialEncabezadoDTO
            {
                scoring = encabezado.id_scoring,
                tipo_subproducto = encabezado.id_tipo_sub_producto,
                monto_otorgado_original = encabezado.monto_otorgado,
                plazo_meses = encabezado.plazo,
                tasa = encabezado.tasa,
                condiciones_organismo_decisor = encabezado.condiciones_organismo_decisor,
                codigo_oficina = encabezado.codigo_oficina_bbva,
                descripcion_oficina = encabezado.descripcion_oficina_bbva,
                codigo_asesor = encabezado.codigo_asesor_bbva,
                correo_declarativo_original = encabezado.correo_declarativo,
                telefono_declarativo_original = encabezado.telefono_declarativo
            };

            return dto;
        }

        public async Task<object> GetControles()
        {

            return new
            {
                motivo_cierre = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoCierre),
                motivo_devolucion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion),
                tipo_documento_id = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId),
                situacion_laboral = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.SituacionLaboral),
                tipo_inmueble = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoInmueble),
                departamento = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Departamento),
                municipio = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Municipio),
                estatus_general = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstatusGeneral),
                canal_contacto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CanalContacto),
                resultado_contacto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.ResultadoContacto),
                tipo_credito = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoCredito),
            };
        }

        public async Task<GuardarDevolucionVbComercialDTO> Guardar(GuardarDevolucionVbComercialDTO model, int usuarioId, string actividadId)
        {

            try
            {
                var formulario = model.formulario;

                // MAPEAMOS EL FORMULARIO A LA ENTIDAD
                var entityFormulario = Mapper.Map<devolucion_vb_comercial_entity>(
                    new devolucion_vb_comercial
                    {
                        id = formulario.id,
                        id_expediente = formulario.id_expediente,
                        cliente_desiste = formulario.cliente_desiste,
                        motivo_cierre = formulario.motivo_cierre,
                        observaciones = formulario.observaciones
                    });

                // ACTUALIZAMOS DATOS DE LA ACTIVIDAD
                var resultado = await DevolucionVbComercialRepositoryProvider.Guardar(entityFormulario, usuarioId);

                // REGISTRAMOS EN LA BITACORA
                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(entityFormulario.id_expediente, actividadId);
                bitacora bitacora = new bitacora
                {
                    id_expediente = entityFormulario.id_expediente,
                    id_actividad = actividadId,
                    id_usuario = usuarioId,
                    fecha_alta = DateTime.Now,
                    observaciones = entityFormulario.observaciones ?? string.Empty,
                    is_active = true,
                    row_status = true
                };

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.UpdateWithoutSaveChanges(bitacora, usuarioId);
                }
                else
                {
                    BitacoraApplicacionProvider.CreateWithoutSaveChanges(bitacora, usuarioId);
                }

                // GUARDAMOS TODOS LOS CAMBIOS
                await _dbContext.SaveChangesAsync();

                model.formulario.id = resultado.id;

                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id, bool confirmarCierre)
        {
            var entity = await DevolucionVbComercialRepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null)
            {
                throw new InvalidOperationException("Debe guardar la información antes de avanzar la actividad.");
            }

            // REGLAS DE SEGURIDAD BACKEND PARA EL CIERRE IRREVERSIBLE
            if (entity.cliente_desiste == true)
            {
                if (string.IsNullOrWhiteSpace(entity.motivo_cierre))
                {
                    throw new BusinessException("El motivo de cierre es obligatorio cuando el cliente desiste.");
                }

                if (!confirmarCierre)
                {
                    throw new InvalidOperationException("Falta el parámetro de confirmación explícita para proceder con el cierre terminal del Folio.");
                }
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            // Mapeo dinámico de las transiciones del gateway XPDL según HU
            string nombreTransicionEsperada = entity.cliente_desiste == true
                ? "TR_022"
                : "TR_021";

            string transitionID = listTransitions
                .Where(q => q.name == nombreTransicionEsperada)
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionID))
            {
                throw new InvalidOperationException($"No se encontró la transición '{nombreTransicionEsperada}' configurada en el motor para esta actividad.");
            }

            var resultadoAvanzar = await WorkflowApplicationProvider.AvanzarActividad(transitionID, folio, usuario_id);

            return resultadoAvanzar;
        }

        private async Task<devolucion_vb_comercial?> GetByExpediente(long id_expediente)
        {
            var entity = await DevolucionVbComercialRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<devolucion_vb_comercial?>(entity) ?? new devolucion_vb_comercial { id_expediente = id_expediente };
        }
    }
}
