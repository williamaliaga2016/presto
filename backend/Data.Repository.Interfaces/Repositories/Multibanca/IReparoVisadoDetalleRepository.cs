using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReparoVisadoDetalleRepository
    {
        Task<reparo_visado_detalle_entity> GetByExpediente(long id_expediente);
        Task MarkSubsanado(int id_visar_operacion, int userId);
    }
}
