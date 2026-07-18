using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IReparoCdrDetalleApplication : IMultibancaGenericApplication<reparo_cdr_detalle>
    {
        Task<reparo_cdr_detalle?> GetByExpediente(long id_expediente);
    }
}
