using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RevisarDocumentosInmuebleApplication
        : MultibancaGenericApplication<revisar_documentos_inmueble, revisar_documentos_inmueble_entity, IRevisarDocumentosInmuebleRepository>,
          IRevisarDocumentosInmuebleApplication
    {
        // Codigos del catalogo de documentos del Expediente Digital (Mongo) que se
        // consideran minimos obligatorios para esta actividad (CA8/CA9 de BBV-47).
        // IMPORTANTE: son codigos de referencia (siguiendo el patron usado en
        // ValidarIntegracionApplication con "CartaCondicionado"/"CertificacionEntidad");
        // validar contra el catalogo real de documentos (cat_expediente_digital_documentos)
        // antes de llevar esto a un ambiente productivo.
        private static readonly string[] CodigosDocumentosObligatorios =
        {
            "PromesaCompraventa",
            "Escritura",
            "CTL"
        };

        private readonly IRevisarDocumentosInmuebleRepository RevisarDocumentosInmuebleRepositoryProvider;
        private readonly IValidarInformacionRepository ValidarInformacionRepositoryProvider;
        private readonly IEncabezadoApplication EncabezadoApplicationProvider;
        private readonly IExpedienteDigitalApplication ExpedienteDigitalApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicationProvider;
        private readonly IMapper Mapper;

        public RevisarDocumentosInmuebleApplication(
            MultibancaDBContext multibancaDBContext,
            IRevisarDocumentosInmuebleRepository revisarDocumentosInmuebleRepository,
            IValidarInformacionRepository validarInformacionRepository,
            IEncabezadoApplication encabezadoApplication,
            IExpedienteDigitalApplication expedienteDigitalApplication,
            ICommonApplication commonApplication,
            IWorkflowApplication workflowApplication,
            IBitacoraApplication bitacoraApplication,
            IMapper mapper)
            : base(multibancaDBContext, revisarDocumentosInmuebleRepository, mapper)
        {
            RevisarDocumentosInmuebleRepositoryProvider = revisarDocumentosInmuebleRepository;
            ValidarInformacionRepositoryProvider = validarInformacionRepository;
            EncabezadoApplicationProvider = encabezadoApplication;
            ExpedienteDigitalApplicationProvider = expedienteDigitalApplication;
            CommonApplicationProvider = commonApplication;
            WorkflowApplicationProvider = workflowApplication;
            BitacoraApplicationProvider = bitacoraApplication;
            Mapper = mapper;
        }

        public async Task<object> GetByExpediente(long idExpediente)
        {
            var entity = await RevisarDocumentosInmuebleRepositoryProvider.GetByExpediente(idExpediente);

            var formulario = Mapper.Map<revisar_documentos_inmueble?>(entity)
                ?? new revisar_documentos_inmueble
                {
                    id_expediente = idExpediente
                };

            // Información General (solo lectura)
            var encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(
                idExpediente,
                Constants.ActividadesBBVA.RevisarDocs);

            var informacionGeneral = new
            {
                editable = false,
                responsable = encabezado.responsable,
                actividad = encabezado.actividad,
                estado = encabezado.estado,
                usuario_asignado = encabezado.usuario_asignado,
                fecha_alta = encabezado.fecha_alta,
                fecha_asignacion = encabezado.fecha_asignacion
            };

            // Datos heredados de Validar Información
            var heredados = await ValidarInformacionRepositoryProvider.GetByExpediente(idExpediente)
                ?? new validar_informacion_bbva
                {
                    id_expediente = idExpediente
                };

            var datosHeredados = new
            {
                editable = false,
                datos_titular = new
                {
                    tipo_identificacion = heredados.tipo_id_t1,
                    numero_identificacion = heredados.numero_id_t1,
                    nombre_completo_t1 = heredados.nombre_completo_t1,
                    situacion_laboral = heredados.situacion_laboral_t1
                },
                datos_inmueble = new
                {
                    tipo_inmueble = heredados.tipo_inmueble,
                    estado_inmueble = heredados.estado_inmueble,
                    constructora = heredados.constructora,
                    descripcion_proyecto = heredados.descripcion_proyecto,
                    departamento_inmueble = heredados.departamento_inmueble,
                    municipio_inmueble = heredados.municipio_inmueble
                },
                datos_credito = new
                {
                    tipo_credito = heredados.tipo_credito,
                    tiene_garantia = heredados.tiene_garantia,
                    monto_otorgado_vi = heredados.monto_otorgado_vi
                },
                condiciones_financieras = new
                {
                    scoring = encabezado.id_scoring,
                    subproducto = encabezado.id_tipo_sub_producto,
                    monto_otorgado = encabezado.monto_otorgado,
                    plazo_meses = encabezado.plazo,
                    tasa = encabezado.tasa,
                    condiciones_organismo_decisor = encabezado.condiciones_organismo_decisor
                }
            };

            // ==========================================================
            // BYPASS TEMPORAL MONGO
            // ==========================================================

            object documentos = Array.Empty<object>();

            bool documentosCompletos = true;

            List<string> documentosFaltantes = new();

            //try
            //{
            //    documentos = await ExpedienteDigitalApplicationProvider
            //        .GetFilesExpedienteDigital(idExpediente);

            //    (documentosCompletos, documentosFaltantes) =
            //        await ValidarDocumentosObligatorios(idExpediente);
            //}
            //catch
            //{
            //    // Sin conexión a Mongo.
            //    // Se continúa el flujo devolviendo documentos vacíos.
            //    documentos = Array.Empty<object>();
            //    documentosCompletos = true;
            //    documentosFaltantes = new List<string>();
            //}

            // ==========================================================

            return new
            {
                informacion_general = informacionGeneral,
                datos_heredados = datosHeredados,
                expediente_digital = documentos,
                documentos_obligatorios = new
                {
                    completos = documentosCompletos,
                    faltantes = documentosFaltantes
                },
                formulario
            };
        }

        public async Task<object> GetControles()
        {
            return new
            {
                motivo_devolucion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion)
            };
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int idUsuario, string idActividad)
        {
            var entity = await RevisarDocumentosInmuebleRepositoryProvider.GetByExpediente(idExpediente);
            if (entity == null)
                throw new InvalidOperationException("No existe registro de Revisar Documentos Inmueble. Debe guardar la actividad antes de avanzar.");

            if (entity.documentos_correctos == null)
                throw new InvalidOperationException("Debe indicar si los documentos son correctos antes de avanzar.");

            if (entity.documentos_correctos == false && string.IsNullOrWhiteSpace(entity.motivo_devolucion))
                throw new InvalidOperationException("Debe seleccionar el motivo de devolución cuando los documentos no son correctos.");

            // CA9: no puede avanzarse como correcto si faltan documentos obligatorios en el
            // Expediente Digital. NOTA: solo valida ausencia de archivo; la marca de
            // "documento_correcto=false" por documento vive en Mongo y no esta modelada hoy
            // en el domain model expediente_digital, por lo que ese caso puntual de CA9 no
            // se puede validar aqui todavia.
            if (entity.documentos_correctos == true)
            {
                var (completos, faltantes) = await ValidarDocumentosObligatorios(idExpediente);
                if (!completos)
                {
                    throw new BusinessException(
                        "No puede avanzar el caso como correcto si existen documentos obligatorios rechazados o faltantes en el expediente. " +
                        $"Faltan: {string.Join(", ", faltantes)}.");
                }
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(idExpediente, idActividad);
            List<xpdl_transition_DTO> transitions = await WorkflowApplicationProvider.GetTransitions(idActividad);

            // Retencion del mismo usuario en la devolucion a Validar Informacion: depende de
            // como este configurado el performer de la transicion 'RevisarDocs_Devolver' en el
            // motor XPDL (no se reimplementa aqui a nivel de codigo).
            string transitionName = entity.documentos_correctos == true
                ? "TR_012"
                : "TR_023";

            string transitionId = transitions
                .Where(x => x.name == transitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrEmpty(transitionId))
                throw new InvalidOperationException($"No se encontró la transición '{transitionName}' para esta actividad.");

            var resultado = await WorkflowApplicationProvider.AvanzarActividad(transitionId, folio, idUsuario);

            RegistrarBitacora(idExpediente, idUsuario, idActividad, entity);

            return resultado;
        }

        private async Task<(bool completos, List<string> faltantes)> ValidarDocumentosObligatorios(long idExpediente)
        {
            var documentosExpediente = await ExpedienteDigitalApplicationProvider.GetFilesExpedienteDigital(idExpediente);
            var catalogoDocumentos = await ExpedienteDigitalApplicationProvider.GetCatalogoDocumentosCompleto();

            var faltantes = new List<string>();

            foreach (var codigo in CodigosDocumentosObligatorios)
            {
                var doc = catalogoDocumentos.FirstOrDefault(c => string.Equals(c.code, codigo, StringComparison.OrdinalIgnoreCase));
                var cargado = doc != null && documentosExpediente.Any(d => d.id_documento == doc.id && d.is_active && d.row_status);

                if (!cargado)
                    faltantes.Add(codigo);
            }

            return (faltantes.Count == 0, faltantes);
        }

        private void RegistrarBitacora(long idExpediente, int idUsuario, string idActividad, revisar_documentos_inmueble_entity entity)
        {
            // La tabla bitacora solo tiene "observaciones" como texto libre (no columnas
            // dedicadas de Decision/Motivo Devolucion), igual que en DevolucionVbComercial
            // y ValidarIntegracion: se arma un texto descriptivo con ambos datos.
            var decision = entity.documentos_correctos == true ? "Documentos Correctos" : "Documentos No Correctos";
            var observaciones = $"{decision}.";

            if (entity.documentos_correctos == false && !string.IsNullOrWhiteSpace(entity.motivo_devolucion))
                observaciones += $" Motivo devolución: {entity.motivo_devolucion}.";

            if (!string.IsNullOrWhiteSpace(entity.observaciones))
                observaciones += $" {entity.observaciones}";

            BitacoraApplicationProvider.Create(new bitacora
            {
                id_expediente = idExpediente,
                id_actividad = idActividad,
                id_usuario = idUsuario,
                fecha_alta = DateTime.Now,
                observaciones = observaciones,
                is_active = true,
                row_status = true
            }, idUsuario);
        }
    }
}
