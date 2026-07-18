using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

/// <summary>
/// Orquesta las reglas de negocio del registro de contactos BBVA.
/// </summary>
public class RegistroContactoApplication(
    IRegistroContactoRepository repository,
    ICommonApplication commonApplication)
    : IRegistroContactoApplication
{
    /// <inheritdoc />
    public async Task<List<registro_contacto_bbva>> GetByExpediente(long idExpediente)
    {
        if (idExpediente <= 0)
            throw new InvalidOperationException("El expediente es obligatorio.");

        return await repository.GetByExpediente(idExpediente);
    }

    /// <inheritdoc />
    public async Task<object> GetControles()
    {
        return new
        {
            canal_contacto = await commonApplication.GetCatalogoByType(Constants.Catalogo.CanalContacto),
            resultado_contacto = await commonApplication.GetCatalogoByType(Constants.Catalogo.ResultadoContacto),
            detalle_contacto = await commonApplication.GetCatalogoByTypeWithParentCode(Constants.Catalogo.DetalleContacto),
        };
    }

    /// <inheritdoc />
    public async Task<registro_contacto_bbva> Crear(registro_contacto_bbva request, int userId)
    {
        Validar(request);
        request.fecha_contacto = request.fecha_contacto == default
            ? DateTime.Today
            : request.fecha_contacto.Date;

        return await repository.Crear(request, userId);
    }

    /// <summary>
    /// Valida los campos requeridos para persistir un contacto visible en historico.
    /// </summary>
    /// <param name="request">Contacto recibido desde el frontend.</param>
    private static void Validar(registro_contacto_bbva request)
    {
        if (request.id_expediente <= 0)
            throw new InvalidOperationException("El expediente es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.id_actividad))
            throw new InvalidOperationException("La actividad es obligatoria.");

        if (string.IsNullOrWhiteSpace(request.canal_contacto))
            throw new InvalidOperationException("El canal de contacto es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.resultado_contacto))
            throw new InvalidOperationException("El resultado de contacto es obligatorio.");

        if (!request.inmueble_definido.HasValue)
            throw new InvalidOperationException("Debe indicar si el inmueble esta definido.");
    }
}
