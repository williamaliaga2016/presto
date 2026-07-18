using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface ITransitionProcessorApplication
    {
        Task CreateTransition(xpdl_transition_DTO transition);

        Task<List<xpdl_transition_DTO>> GetTransitions(string activityId);

        Task<List<xpdl_transition_DTO>> GetFromTransitions(string toActivity);
    }
}