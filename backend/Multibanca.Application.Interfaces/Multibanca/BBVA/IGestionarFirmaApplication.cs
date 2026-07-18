using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

public interface IGestionarFirmaApplication
{
    Task<gestionar_firma_bbva?> GetByExpediente(long idExpediente);
    Task<gestionar_firma_bbva> Guardar(gestionar_firma_bbva request, int userId);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<object> GetControles(long idExpediente);
    Task<object> GetFormularioConEncabezado(long idExpediente);
}