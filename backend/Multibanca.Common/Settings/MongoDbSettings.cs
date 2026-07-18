namespace Multibanca.Common.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string ExpedienteDigitalCollectionName { get; set; } = "expediente_digital";
        public string SequenceCollectionName { get; set; } = "sequences";
    }
}
