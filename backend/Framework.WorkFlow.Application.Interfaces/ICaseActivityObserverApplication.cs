using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface ICaseActivityObserverApplication
    {
        Task Notify(notify_item_DTO item);
    }
}