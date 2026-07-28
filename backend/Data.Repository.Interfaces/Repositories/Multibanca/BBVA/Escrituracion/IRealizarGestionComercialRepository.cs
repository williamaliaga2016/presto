using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarGestionComercialRepository : IMultibancaGenericRepository<realizar_gestion_comercial_entity>
{
    Task<realizar_gestion_comercial_entity?> GetByExpediente(long idExpediente);
}
