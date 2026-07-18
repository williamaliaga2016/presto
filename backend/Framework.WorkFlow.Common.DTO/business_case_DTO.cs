namespace Framework.WorkFlow.Common.DTO
{
    public class business_case_DTO
    {
        public long case_id { get; set; }

        public string? title { get; set; }

        public string? state { get; set; }

        public Guid workflow_instance_id { get; set; }

        public string? workflow_process_id { get; set; }

        public string? created_by { get; set; }

        public DateTime? created { get; set; }

        public List<business_activity_DTO>? current_activities { get; set; }
    }
}