using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface IRealizarVBFinalAbogadoRepository : IMultibancaGenericRepository<realizar_vb_final_abogado_entity>
{
    Task<realizar_vb_final_abogado_entity?> GetByExpediente(long idExpediente);
}
