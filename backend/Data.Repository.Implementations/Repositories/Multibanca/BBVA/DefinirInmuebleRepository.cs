using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

/// <summary>
/// Persistencia EF de la actividad BBV-44 Definir Inmueble.
/// </summary>
public class DefinirInmuebleRepository(MultibancaDBContext context) : IDefinirInmuebleRepository
{
    /// <inheritdoc />
    public async Task<definir_inmueble_bbva?> GetByExpediente(long idExpediente)
    {
        return await context.DefinirInmuebleBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<definir_inmueble_bbva> Guardar(definir_inmueble_bbva request, int userId)
    {
        var existing = await context.DefinirInmuebleBbva
            .FirstOrDefaultAsync(q =>
                q.id_expediente == request.id_expediente &&
                q.is_active &&
                q.row_status);

        request.id_actividad = Constants.ActividadesBBVA.DefinirInmueble;

        if (request.cliente_cuenta_inmueble_definido == true &&
            string.Equals(request.estatus_general, "SIN_INM", StringComparison.OrdinalIgnoreCase))
        {
            request.estatus_general = "LISTO";
        }

        if (existing is null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            context.DefinirInmuebleBbva.Add(request);
            await context.SaveChangesAsync();
            return request;
        }

        existing.id_actividad = request.id_actividad;
        existing.cliente_cuenta_inmueble_definido = request.cliente_cuenta_inmueble_definido;
        existing.constructora = request.constructora;
        existing.fecha_estimada_entrega = request.fecha_estimada_entrega;
        existing.estatus_general = request.estatus_general;
        existing.motivo_devolucion = request.motivo_devolucion;
        existing.observaciones = request.observaciones;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        await context.SaveChangesAsync();
        return existing;
    }
}
