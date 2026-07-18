namespace Framework.WorkFlow.Common.DTO
{
    public class xpdl_transition_DTO
    {
        public string? transition_id { get; set; }

        public string? name { get; set; }

        public string? from_activity { get; set; }

        public string? to_activity { get; set; }

        public string? condition { get; set; }

        public string? workflow_process_id { get; set; }
    }
}