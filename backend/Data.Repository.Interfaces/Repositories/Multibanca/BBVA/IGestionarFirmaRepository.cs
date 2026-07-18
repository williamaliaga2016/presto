using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IGestionarFirmaRepository
{
    Task<gestionar_firma_bbva?> GetByExpediente(long idExpediente);
    Task<gestionar_firma_bbva> Guardar(gestionar_firma_bbva request, int userId);
}