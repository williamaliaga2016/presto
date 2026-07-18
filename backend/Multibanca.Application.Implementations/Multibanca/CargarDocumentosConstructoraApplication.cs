using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CargarDocumentosConstructoraApplication
        : MultibancaGenericApplication<cargar_documentos_constructora, cargar_documentos_constructora_entity, ICargarDocumentosConstructoraRepository>,
          ICargarDocumentosConstructoraApplication
    {
        private readonly ICargarDocumentosConstructoraRepository CargarDocumentosConstructoraRepositoryProvider;
        private readonly IExpedienteDigitalMongoRepository ExpedienteDigitalMongoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CargarDocumentosConstructoraApplication(
            MultibancaDBContext multibancaDBContext,
            ICargarDocumentosConstructoraRepository cargarDocumentosConstructoraRepository,
            IExpedienteDigitalMongoRepository expedienteDigitalMongoRepository,
            IWorkflowApplication workflowApplication,
            IMapper mapper)
            : base(multibancaDBContext, cargarDocumentosConstructoraRepository, mapper)
        {
            CargarDocumentosConstructoraRepositoryProvider = cargarDocumentosConstructoraRepository;
            ExpedienteDigitalMongoRepositoryProvider = expedienteDigitalMongoRepository;
            WorkflowApplicationProvider = workflowApplication;
            Mapper = mapper;
        }

        public async Task<cargar_documentos_constructora?> GetByExpediente(long idExpediente)
        {
            var entity = await CargarDocumentosConstructoraRepositoryProvider.GetByExpediente(idExpediente);
            return Mapper.Map<cargar_documentos_constructora?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int idUsuario, string idActividad)
        {
            var entity = await CargarDocumentosConstructoraRepositoryProvider.GetByExpediente(idExpediente);
            if (entity == null)
                throw new InvalidOperationException("No existe registro de Cargar Documentos Constructora. Debe guardar la actividad antes de avanzar.");

            if (!entity.avanzar_validar_documentos)
                throw new InvalidOperationException("Debe marcar la confirmación de documentos antes de avanzar.");

            if (string.IsNullOrWhiteSpace(entity.observaciones))
                throw new InvalidOperationException("Debe ingresar observaciones antes de avanzar.");

            var archivos = await ExpedienteDigitalMongoRepositoryProvider.GetFilesByActividad(idExpediente, Constants.ActividadesBBVA.DocsConstructora);
            bool tienePromesa = archivos.Any(x => x.id_documento == Constants.DocumentosBBVA.PromesaCompraventa && x.is_active);
            if (!tienePromesa)
                throw new InvalidOperationException("Debe cargar la Promesa de Compraventa en el Expediente Digital antes de avanzar.");

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(idExpediente, idActividad);
            List<xpdl_transition_DTO> transitions = await WorkflowApplicationProvider.GetTransitions(idActividad);

            // Validamos si ya comenzo la actividad de comenzar firma
            bool yaInicioActividadGestionarFirma = await WorkflowApplicationProvider.ExisteActividadFolio(
                idExpediente,
                Constants.ActividadesBBVA.GestionarFirma);

            // Validamos si ya realizo la firma, si es asi, va a la actividad
            string nombreTransicionEsperada = yaInicioActividadGestionarFirma == true
                    ? "TR_008"
                    : "TR_007";

            string transitionId = transitions
                .Where(x => x.name == nombreTransicionEsperada)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            return await WorkflowApplicationProvider.AvanzarActividad(transitionId, folio, idUsuario);
        }
    }
}
