using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class AsignarEstudioTitulosApplication : MultibancaGenericApplication<asignar_estudio_titulos, asignar_estudio_titulos_entity, IAsignarEstudioTitulosRepository>, IAsignarEstudioTitulosApplication
    {
        private readonly IAsignarEstudioTitulosRepository AsignarEstudioTitulosRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public AsignarEstudioTitulosApplication(
            MultibancaDBContext _multibancaDBContext,
            IAsignarEstudioTitulosRepository _asignarEstudioTitulosRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _asignarEstudioTitulosRepository, _mapper)
        {
            AsignarEstudioTitulosRepositoryProvider = _asignarEstudioTitulosRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<asignar_estudio_titulos?> GetByExpediente(long id_expediente)
        {
            var entity = await AsignarEstudioTitulosRepositoryProvider.GetByExpedienteActividad(
                id_expediente,
                Constants.Actividades.AsignarEstudioTitulos);

            return Mapper.Map<asignar_estudio_titulos?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await AsignarEstudioTitulosRepositoryProvider.GetByExpedienteActividad(expediente_id,
                Constants.Actividades.AsignarEstudioTitulos);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Tasación para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
           
            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}

