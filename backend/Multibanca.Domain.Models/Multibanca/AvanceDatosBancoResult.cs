using System.Collections.Generic;

namespace Multibanca.Domain.Models.Multibanca
{
    public class AvanceDatosBancoResult
    {
        public bool status { get; set; }
        public string? next_activity { get; set; }
        public List<string> next_activities { get; set; } = new List<string>();
        public string message { get; set; } = string.Empty;
    }
}
