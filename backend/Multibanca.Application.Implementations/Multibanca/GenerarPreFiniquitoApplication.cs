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
    public class GenerarPreFiniquitoApplication : MultibancaGenericApplication<generar_prefiniquito, generar_prefiniquito_entity, IGenerarPreFiniquitoRepository>, IGenerarPreFiniquitoApplication
    {
        private readonly IGenerarPreFiniquitoRepository GenerarPreFiniquitoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GenerarPreFiniquitoApplication(
            MultibancaDBContext multibancaDBContext,
            IGenerarPreFiniquitoRepository generarPreFiniquitoRepositoryProvider, 
            IWorkflowApplication workflowApplicationProvider, 
            IMapper mapper) : base(multibancaDBContext,generarPreFiniquitoRepositoryProvider,mapper)
        {
            GenerarPreFiniquitoRepositoryProvider = generarPreFiniquitoRepositoryProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = mapper;
        }
        public async Task<generar_prefiniquito?> GetByExpediente(long id_expediente)
        {
            var entity = await GenerarPreFiniquitoRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<generar_prefiniquito?>(entity);
        }
        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await GenerarPreFiniquitoRepositoryProvider.GetByExpediente(expediente_id);


            string transitionsID = listTransitions.Select(q => q.transition_id).FirstOrDefault() ?? "";
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
