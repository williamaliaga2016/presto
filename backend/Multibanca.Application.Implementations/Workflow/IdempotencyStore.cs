using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.Application.Interfaces.Workflow;

namespace Multibanca.Application.Implementations.Workflow
{
    public class IdempotencyStore : IIdempotencyStore
    {
        private readonly MultibancaDBContext _context;

        public IdempotencyStore(MultibancaDBContext context)
        {
            _context = context;
        }

        public async Task<IdempotencyRecord?> Get(string key)
        {
            var entity = await _context.idempotency_keys
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.key == key);

            if (entity is null)
                return null;

            return new IdempotencyRecord(entity.key, entity.id_expediente, entity.response_snapshot);
        }

        public async Task Save(string key, long idExpediente, string responseSnapshot)
        {
            var entity = new idempotency_key_entity
            {
                key = key,
                id_expediente = idExpediente,
                response_snapshot = responseSnapshot,
                created_at = DateTime.UtcNow
            };

            _context.idempotency_keys.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
