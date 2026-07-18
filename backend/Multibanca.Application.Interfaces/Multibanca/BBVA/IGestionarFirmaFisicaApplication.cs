using Multibanca.Domain.Models.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

public interface IGestionarFirmaFisicaApplication
{
    Task<gestionar_firma_fisica_bbva?> GetByExpediente(long idExpediente);
    Task<gestionar_firma_fisica_bbva> Guardar(gestionar_firma_fisica_bbva request, int userId);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<object> GetControles(long idExpediente);
    Task<object> GetFormularioConEncabezado(long idExpediente);
}
