using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IReparoVisadoDetalleApplication
    {
        Task<List<reparo_visado_detalle>> GetByExpediente(long id_expediente);
        Task MarkSubsanado(int id_visar_operacion, int userId);
    }
}
