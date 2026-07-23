using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IRealizarRecepcionBoletaApplication : IMultibancaGenericApplication<realizar_recepcion_boleta>
{
    Task<object?> GetByExpediente(long idExpediente);
    Task<object> GetControles();
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<realizar_recepcion_boleta> EjecutarVUR(long idExpediente, int userId);
}
