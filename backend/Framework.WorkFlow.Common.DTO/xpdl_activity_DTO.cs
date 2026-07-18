namespace Framework.WorkFlow.Common.DTO
{
    public class xpdl_activity_DTO
    {
        public string? activity_id { get; set; }

        public string? workflow_process_id { get; set; }

        public string? display_name { get; set; }

        public string? name { get; set; }

        public string? task_type { get; set; }

        public string? task_form_type { get; set; }

        public string? task_form_uri { get; set; }

        public string? performer { get; set; }

        public string? sub_flow_id { get; set; }
    }
}