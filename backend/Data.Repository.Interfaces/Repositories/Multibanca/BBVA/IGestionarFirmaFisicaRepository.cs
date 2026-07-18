using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IGestionarFirmaFisicaRepository
{
    Task<gestionar_firma_fisica_bbva?> GetByExpediente(long idExpediente);
    Task<gestionar_firma_fisica_bbva> Guardar(gestionar_firma_fisica_bbva request, int userId);
}
