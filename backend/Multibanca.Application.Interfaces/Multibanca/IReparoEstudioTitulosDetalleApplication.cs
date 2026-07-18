using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IReparoEstudioTitulosDetalleApplication : IMultibancaGenericApplication<reparo_estudio_titulos_detalle>
    {
        Task<List<reparo_estudio_titulos_detalle>> GetByExpediente(long id_expediente);
    }
}
