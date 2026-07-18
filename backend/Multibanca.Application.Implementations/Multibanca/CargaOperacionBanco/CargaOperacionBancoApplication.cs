using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoApplication : MultibancaGenericApplication<carga_operacion_banco, carga_operacion_banco_entity, ICargaOperacionBancoRepository>, ICargaOperacionBancoApplication
    {
        private readonly ICargaOperacionBancoRepository CargaOperacionBancoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CargaOperacionBancoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICargaOperacionBancoRepository _cargaOperacionBancoRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper)
            : base(_multibancaDBContext, _cargaOperacionBancoRepository, _mapper)
        {
            CargaOperacionBancoRepositoryProvider = _cargaOperacionBancoRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<carga_operacion_banco?> GetByExpediente(long id_expediente)
        {
            carga_operacion_banco_entity entity = await CargaOperacionBancoRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<carga_operacion_banco?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            List<AssignActivityDTO> listAssignActivityDTO =
                await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);

            return listAssignActivityDTO;
        }
    }
}