using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IVerificarReparoDatosOperacionRepository : IMultibancaGenericRepository<verificar_reparo_datos_operacion_entity>, IDisposable
    {
        Task<verificar_reparo_datos_operacion_entity?> GetByExpediente(long id_expediente);
    }
}
