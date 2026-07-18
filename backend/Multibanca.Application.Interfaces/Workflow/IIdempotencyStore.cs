namespace Multibanca.Application.Interfaces.Workflow
{
    public record IdempotencyRecord(string Key, long IdExpediente, string ResponseSnapshot);

    public interface IIdempotencyStore
    {
        Task<IdempotencyRecord?> Get(string key);
        Task Save(string key, long idExpediente, string responseSnapshot);
    }
}
