using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarEstudioTitulosRepository:IMultibancaGenericRepository<generar_estudio_titulos_entity>, IDisposable
    {
        Task<generar_estudio_titulos_entity?> GetByExpediente(long id_expediente);
    }
}
