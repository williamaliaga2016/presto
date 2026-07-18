using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.DatosOperacion
{
    public class DatosOperacionApplication :
        MultibancaGenericApplication<
            datos_operacion,
            datos_operacion_entity,
            IDatosOperacionRepository>,
        IDatosOperacionApplication
    {
        private readonly IDatosOperacionRepository RepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider; 
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly IMapper Mapper;

        public DatosOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            IDatosOperacionRepository _repository,
            IWorkflowApplication _workflowApplication,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            WorkflowApplicationProvider = _workflowApplication;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            Mapper = _mapper;
        }

        public async Task<datos_operacion> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<datos_operacion>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await RepositoryProvider.GetByExpediente(expediente_id);
            bool envioReparo = entity.enviar_reparo ?? false;

            string transitionsID;

            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                var cargaOperacion = await CargaOperacionBancoDatosOperacionApplicationProvider.GetByExpediente(expediente_id);
                if (cargaOperacion.nro_piloto == null || cargaOperacion.nro_piloto == 0)
                {
                    transitionsID = listTransitions
                    .Where(x =>
                        x.name == "DatosOperacion_ReparoNO_INCOMPLETO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
                }
                else
                {
                    transitionsID = listTransitions
                    .Where(x =>
                        x.name == "DatosOperacion_RegistrarTasacion_ReparoNO_COMPLETO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
                }
                
            }

            List<AssignActivityDTO> listAssignActivityDTO =
                await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);

            return listAssignActivityDTO;
        }
    }
}
