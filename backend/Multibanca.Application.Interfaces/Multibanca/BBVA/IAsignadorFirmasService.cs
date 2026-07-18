using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

public interface IAsignadorFirmasService
{
    Task<object> Calcular(CalcularAsignacionRequest request);
}
