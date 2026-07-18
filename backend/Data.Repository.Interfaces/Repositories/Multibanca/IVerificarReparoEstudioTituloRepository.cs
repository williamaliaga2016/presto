using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IVerificarReparoEstudioTituloRepository : IMultibancaGenericRepository<verificar_reparo_estudio_titulo_entity>, IDisposable
    {
        Task<verificar_reparo_estudio_titulo_entity?> GetByExpediente(long id_expediente);
    }
}
