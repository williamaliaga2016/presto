namespace Multibanca.Domain.Models.Multibanca
{
    public class AvanceCalculoGeneracionResult
    {
        public bool status { get; set; }
        public string? next_activity { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
