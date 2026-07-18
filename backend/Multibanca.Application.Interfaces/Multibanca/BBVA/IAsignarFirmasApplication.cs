using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

public interface IAsignarFirmasApplication
{
    Task<object> GetConEncabezado(long idExpediente);
    Task<object> GetControles();
    Task<asignar_firmas_peritos_abogados> Guardar(asignar_firmas_peritos_abogados request, int userId);
    Task<object> Calcular(CalcularAsignacionRequest request);
    Task<AsignarFirmasAvanzarResponseDTO> Avanzar(long idExpediente, int userId);
}
