using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IValidarInformacionRepository
{
    Task<validar_informacion_bbva?> GetByExpediente(long idExpediente);
    Task<validar_informacion_bbva> Guardar(validar_informacion_bbva request, int userId);
    Task<IReadOnlyList<titular_bbva>> GetTitulares(long idExpediente);
    Task<titular_bbva> AgregarTitular(titular_bbva request, int userId);
    Task<bool> ActualizarDatosCredito(validar_informacion_bbva request, int userId);
    Task<bool> ActualizarCondicionesFinancieras(validar_informacion_bbva request, int userId);
}
