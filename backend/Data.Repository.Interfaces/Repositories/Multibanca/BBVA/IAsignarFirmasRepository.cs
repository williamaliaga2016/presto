using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IAsignarFirmasRepository
{
    Task<asignar_firmas_peritos_abogados?> GetByExpediente(long idExpediente);
    Task<asignar_firmas_peritos_abogados> Guardar(asignar_firmas_peritos_abogados request, int userId);
}

