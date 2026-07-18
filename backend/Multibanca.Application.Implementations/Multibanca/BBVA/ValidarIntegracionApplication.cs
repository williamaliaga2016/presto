using AutoMapper;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca.BBVA;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Multibanca.DTO.Multibanca.BBVA;
using Newtonsoft.Json;

namespace Multibanca.Application.Implementations.Multibanca.BBVA
{
    public class ValidarIntegracionApplication : IValidarIntegracionApplication
    {
        private readonly IValidarIntegracionRepository ValidarIntegracionRepositoryProvider;
        private readonly IValidarInformacionRepository ValidarInformacionRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IExpedienteDigitalApplication ExpedienteDigitalApplicationProvider; 
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IEncabezadoApplication EncabezadoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IMapper Mapper;
        private readonly MultibancaDBContext _dbContext;

        public ValidarIntegracionApplication(
            MultibancaDBContext _multibancaDBContext,
            IValidarIntegracionRepository _validarIntegracionRepository,
            IValidarInformacionRepository _validarInformacionRepository,
            IWorkflowApplication _workflowApplication,
            ICommonApplication commonApplication,
            IEncabezadoApplication encabezadoApplication,
            IExpedienteDigitalApplication _expedienteDigitalApplication,
            IMapper _mapper,
            IBitacoraApplication _bitacoraApplication
        )
        {
            _dbContext = _multibancaDBContext;
            ValidarIntegracionRepositoryProvider = _validarIntegracionRepository;
            ValidarInformacionRepositoryProvider = _validarInformacionRepository;
            WorkflowApplicationProvider = _workflowApplication;
            ExpedienteDigitalApplicationProvider = _expedienteDigitalApplication;
            CommonApplicationProvider = commonApplication;
            EncabezadoApplicationProvider = encabezadoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            Mapper = _mapper;
        }

        public async Task<ValidarIntegracionDocumentosDTO> GetByExpedienteConEncabezado(long id_expediente)
        {
            var dto = new ValidarIntegracionDocumentosDTO();

            var validarIntegracionDomain = await GetByExpediente(id_expediente) ?? new validar_integracion_documentos();

            // Formulario principal
            dto.formulario = new ValidarIntegracionDocumentosFormularioDTO
            {
                id = validarIntegracionDomain.id,
                id_expediente = validarIntegracionDomain.id_expediente,
                id_actividad = validarIntegracionDomain.id_actividad,
                documentos_correctos = validarIntegracionDomain.documentos_correctos,
                credito_condicionado = validarIntegracionDomain.credito_condicionado,
                motivo_devolucion = validarIntegracionDomain.motivo_devolucion,
                observaciones = validarIntegracionDomain.observaciones,
            };

            // Encabezado
            var encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(id_expediente, Constants.ActividadesBBVA.ValidarIntegracion);

            dto.encabezado = new ValidarIntegracionDocumentosEncabezadoDTO
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

            // Datos de Validar Informacion
            var validarInformacionData = await ValidarInformacionRepositoryProvider.GetByExpediente(id_expediente) ?? new validar_informacion_bbva { id_expediente = id_expediente };
            dto.validar_informacion_data = validarInformacionData;

            return dto;
        }

        public async Task<object> GetControles()
        {

            return new
            {
                motivoDevolucion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion),
                tipo_documento_id = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId),
                situacion_laboral = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.SituacionLaboral),
                tipo_inmueble = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoInmueble),
                departamento = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Departamento),
                municipio = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Municipio),
                estatus_general = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstatusGeneral),
                motivo_devolucion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion),
                canal_contacto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CanalContacto),
                resultado_contacto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.ResultadoContacto),
                tipo_credito = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoCredito),
            };
        }

        public async Task<GuardarValidarIntegracionDocumentosDTO> Guardar(GuardarValidarIntegracionDocumentosDTO model, int usuarioId, string actividadId)
        {

            try
            {
                var formulario = model.formulario;

                // MAPEAMOS EL FORMULARIO A LA ENTIDAD
                var entityFormulario = Mapper.Map<validar_integracion_documentos_entity>(
                    new validar_integracion_documentos
                    {
                        id = formulario.id,
                        id_expediente = formulario.id_expediente,
                        documentos_correctos = formulario.documentos_correctos,
                        credito_condicionado = formulario.credito_condicionado,
                        motivo_devolucion = formulario.motivo_devolucion,
                        observaciones = formulario.observaciones
                    });

                // RECUPERAMOS Y VALIDAMOS QUE HAYA DATOS EN LA ACTIVIDAD validar_informacion_bbva
                var informacion = JsonConvert.DeserializeObject<validar_informacion_bbva>(model.validar_informacion_data.ToString());

                if (informacion == null) throw new BusinessException("Los datos de validación de información del core son requeridos.");

                // ACTUALIZAMOS DATOS DE LA ACTIVIDAD
                var resultado = await ValidarIntegracionRepositoryProvider.Guardar(entityFormulario, usuarioId);

                // ACTUALIZAMOS DATOS DE VALIDAR INFORMACION
                var condicionesActualizadas = await ValidarInformacionRepositoryProvider.ActualizarCondicionesFinancieras(informacion, usuarioId);

                if (!condicionesActualizadas) throw new InvalidOperationException($"No se encontró información de la actividad validar_informacion del expediente {informacion.id_expediente}.");

                var datosCreditoActualizados = await ValidarInformacionRepositoryProvider.ActualizarDatosCredito(informacion, usuarioId);


                if (!datosCreditoActualizados) throw new InvalidOperationException($"No se encontró información de la actividad validar_informacion del expediente {informacion.id_expediente}.");

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

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int usuario_id, string actividad_id)
        {
            var entity = await ValidarIntegracionRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException("Debe guardar la información antes de avanzar la actividad.");
            }

            // Crédito Condicionado
            if (entity.documentos_correctos == true && entity.credito_condicionado == true)
            {
                // Recuperamos Archivos
                var documentosExpediente = await ExpedienteDigitalApplicationProvider.GetFilesExpedienteDigital(id_expediente);

                // Definimos los códigos
                string codeCarta = "CartaCondicionado";
                string codeCertificacion = "CertificacionEntidad";

                // Recuperamos el catalogo de documentos disponibles en el expediente digital
                var catalogoDocumentos = await ExpedienteDigitalApplicationProvider.GetCatalogoDocumentosCompleto();

                // Recuperamos los archivos cargados que coindican con los documentos a filtrar
                var docCarta = catalogoDocumentos.FirstOrDefault(c => string.Equals(c.code, codeCarta, StringComparison.OrdinalIgnoreCase));
                var docCertificacion = catalogoDocumentos.FirstOrDefault(c => string.Equals(c.code, codeCertificacion, StringComparison.OrdinalIgnoreCase));

                // Validamos que los tipos de documento realmente existan en el catálogo del sistema
                if (docCarta == null || docCertificacion == null)
                {
                    throw new InvalidOperationException("Configuración incompleta: No se encontraron los tipos de documento 'Carta de Condicionado' o 'Certificación Entidad' en el catálogo.");
                }

                // Validamos que los archivos cargados existan dependiendo del caso
                bool tieneCarta = documentosExpediente.Any(d => d.id_documento == docCarta.id && d.is_active && d.row_status);
                bool tieneCertificacion = documentosExpediente.Any(d => d.id_documento == docCertificacion.id && d.is_active && d.row_status);

                if (!tieneCarta || !tieneCertificacion)
                {
                    throw new BusinessException("Debe cargar los documentos 'Carta de Condicionado' y 'Certificación Entidad' antes de avanzar.");
                }
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            // Seleccionar la transición basandonos en documentos_correctos
            string nombreTransicionEsperada = entity.documentos_correctos == true
                ? "TR_020"
                : "TR_019";

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

        private async Task<validar_integracion_documentos?> GetByExpediente(long id_expediente)
        {
            var entity = await ValidarIntegracionRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<validar_integracion_documentos?>(entity) ?? new validar_integracion_documentos { id_expediente = id_expediente };
        }

        #region INTERVINIENTE

        public async Task<List<IntervinienteDTO>> GetIntervinientes(long idExpediente)
        {
            var entities = await ValidarIntegracionRepositoryProvider.GetIntervinientes(idExpediente);

            var domains = entities
                .Select(entity => Mapper.Map<interviniente_bbva>(entity))
                .ToList();

            var tiposDocumento = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);

            return domains.Select(x => new IntervinienteDTO
            {
                id = x.id,
                id_expediente = x.id_expediente,
                nombre_completo = x.nombre_completo,
                tipo_identificacion = x.tipo_identificacion,
                tipo_identificacion_descripcion = tiposDocumento
                    .FirstOrDefault(c => c.code == x.tipo_identificacion)?.description ?? "",
                numero_identificacion = x.numero_identificacion,
                telefono = x.telefono,
                correo_electronico = x.correo_electronico
            }).ToList();
        }

        public async Task<IntervinienteDTO> GuardarInterviniente(IntervinienteDTO dto, int usuario_id, string actividad_id)
        {
            var domain = new interviniente_bbva
            {
                id = dto.id ?? 0,
                id_expediente = dto.id_expediente,
                id_actividad = actividad_id,
                nombre_completo = dto.nombre_completo ?? string.Empty,
                tipo_identificacion = dto.tipo_identificacion ?? string.Empty,
                numero_identificacion = dto.numero_identificacion ?? string.Empty,
                telefono = dto.telefono,
                correo_electronico = dto.correo_electronico
            };

            var entity = Mapper.Map<interviniente_bbva_entity>(domain);

            var result = await ValidarIntegracionRepositoryProvider.GuardarInterviniente(entity, usuario_id);

            await _dbContext.SaveChangesAsync();

            var resultDomain = Mapper.Map<interviniente_bbva>(result);

            return new IntervinienteDTO
            {
                id = resultDomain.id,
                id_expediente = resultDomain.id_expediente,
                nombre_completo = resultDomain.nombre_completo,
                tipo_identificacion = resultDomain.tipo_identificacion,
                numero_identificacion = resultDomain.numero_identificacion,
                telefono = resultDomain.telefono,
                correo_electronico = resultDomain.correo_electronico
            };
        }

        #endregion
    }
}
