namespace Framework.WorkFlow.Common.DTO
{
    public class business_activity_DTO
    {
        public long case_id { get; set; }

        public string? activity_id { get; set; }

        public string? workflow_process_id { get; set; }

        public string? secuence { get; set; }

        public string? display_name { get; set; }

        public string? name { get; set; }

        public string? performer { get; set; }

        public string? task_type { get; set; }

        public string? task_form_type { get; set; }

        public string? task_form_uri { get; set; }

        public string? status { get; set; }

        public DateTime? date_processed { get; set; }

        public DateTime? date_suspended { get; set; }

        public DateTime? date_incidented { get; set; }

        public DateTime? date_completed { get; set; }

        public string? from_activity { get; set; }

        public string? to_activity { get; set; }

        public string? condition_activity { get; set; }

        public string? sub_flow_id { get; set; }

        public List<xpdl_transition_DTO>? transitions { get; set; }
    }
}