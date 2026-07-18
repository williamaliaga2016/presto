using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GenerarRecursosPagosCbrApplication : MultibancaGenericApplication<generar_recursos_pagos_cbr, generar_recursos_pagos_cbr_entity, IGenerarRecursosPagosCbrRepository>, IGenerarRecursosPagosCbrApplication
    {
        private readonly IGenerarRecursosPagosCbrRepository GenerarRecursosPagosCbrRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GenerarRecursosPagosCbrApplication(
            MultibancaDBContext _multibancaDBContext,
            IGenerarRecursosPagosCbrRepository _generarRecursosPagosCbrRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _generarRecursosPagosCbrRepository, _mapper)
        {
            GenerarRecursosPagosCbrRepositoryProvider = _generarRecursosPagosCbrRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<generar_recursos_pagos_cbr?> GetByExpediente(long id_expediente)
        {
            var entity = await GenerarRecursosPagosCbrRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<generar_recursos_pagos_cbr?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await GenerarRecursosPagosCbrRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Generar Recursos para Pagos CBR para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "GenerarRecursosPagosCBR_RegistrarEscrituraCBR")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Generar Recursos para Pagos CBR."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
