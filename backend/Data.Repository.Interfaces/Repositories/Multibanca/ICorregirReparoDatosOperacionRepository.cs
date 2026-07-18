using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoDatosOperacionRepository
        : IMultibancaGenericRepository<corregir_reparo_datos_operacion_entity>, IDisposable
    {
        Task<corregir_reparo_datos_operacion_entity?> GetByExpediente(long id_expediente);
    }
}
