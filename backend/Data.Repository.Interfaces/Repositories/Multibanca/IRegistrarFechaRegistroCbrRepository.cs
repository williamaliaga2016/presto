using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFechaRegistroCbrRepository : IMultibancaGenericRepository<registrar_fecha_registro_cbr_entity>, IDisposable
    {
        Task<registrar_fecha_registro_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
