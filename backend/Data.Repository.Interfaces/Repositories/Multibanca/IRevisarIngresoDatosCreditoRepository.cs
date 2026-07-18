using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarIngresoDatosCreditoRepository : IMultibancaGenericRepository<revisar_ingreso_datos_credito_entity>, IDisposable
    {
        Task<revisar_ingreso_datos_credito_entity> GetByExpediente(long id_expediente);
    }
}
