using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace Framework.WorkFlow.Application.Implementations
{
    public class CaseActivityObserverApplication : ICaseActivityObserverApplication
    {
        private readonly ICaseActivityTrackingWorkflowRepository CaseActivityTrackingWorkflowRepositoryProvider;

        public CaseActivityObserverApplication(
            ICaseActivityTrackingWorkflowRepository _caseActivityTrackingWorkflowRepository
        )
        {
            CaseActivityTrackingWorkflowRepositoryProvider = _caseActivityTrackingWorkflowRepository;
        }

        public async Task Notify(notify_item_DTO item)
        {
            if (item == null || item.case_item == null)
            {
                return;
            }

            Trace.WriteLine(ParseCase(item.case_item));

            string? xmlCaseInstance = SerializeCase(item.case_item);

            if (string.IsNullOrWhiteSpace(xmlCaseInstance))
            {
                return;
            }

            await CaseActivityTrackingWorkflowRepositoryProvider.TrackActivity(
                item.case_item,
                xmlCaseInstance,
                item.tracking_source
            );
        }

        private static string? SerializeCase(business_case_DTO caseInstance)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(business_case_DTO));

                using MemoryStream memoryStream = new MemoryStream();

                serializer.Serialize(memoryStream, caseInstance);

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (InvalidOperationException ex)
            {
                Trace.WriteLine(ex.Message);
                return null;
            }
        }

        private static string ParseCase(business_case_DTO caseInstance)
        {
            return string.Format(
                "ID = {0}|TITLE = {1}|Status = {2}|WorkflowId = {3}|WorkflowProcessId = {4}",
                caseInstance.case_id,
                caseInstance.title,
                caseInstance.state,
                caseInstance.workflow_instance_id,
                caseInstance.workflow_process_id
            );
        }
    }
}