using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;

namespace Framework.WorkFlow.Application.Implementations
{
    public class TransitionProcessorApplication : ITransitionProcessorApplication
    {
        private readonly IXpdlWorkflowRepository XpdlWorkflowRepositoryProvider;

        public TransitionProcessorApplication(
            IXpdlWorkflowRepository _xpdlWorkflowRepository
        )
        {
            XpdlWorkflowRepositoryProvider = _xpdlWorkflowRepository;
        }

        public async Task CreateTransition(xpdl_transition_DTO transition)
        {
            if (transition == null)
            {
                throw new Exception("La transición XPDL es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(transition.transition_id))
            {
                throw new Exception("El transition_id es obligatorio.");
            }

            await XpdlWorkflowRepositoryProvider.InsertXpdlTransition(transition);
        }

        public async Task<List<xpdl_transition_DTO>> GetTransitions(string activityId)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return new List<xpdl_transition_DTO>();
            }

            /*
                Equivalente antiguo:
                XpdlDataAccess.GetXpdlTransitionsByCriteria(activityId, 1)

                filter = 1:
                from_activity = activityId
            */

            return await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(
                activityId,
                1
            );
        }

        public async Task<List<xpdl_transition_DTO>> GetFromTransitions(string toActivity)
        {
            if (string.IsNullOrWhiteSpace(toActivity))
            {
                return new List<xpdl_transition_DTO>();
            }

            /*
                Equivalente antiguo:
                XpdlDataAccess.GetXpdlTransitionsByCriteria(toActivity, 2)

                filter = 2:
                to_activity = activityId
            */

            return await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(
                toActivity,
                2
            );
        }
    }
}