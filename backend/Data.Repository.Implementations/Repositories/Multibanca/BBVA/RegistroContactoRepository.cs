using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

/// <summary>
/// Persistencia EF para el historico transversal de contactos BBVA.
/// </summary>
public class RegistroContactoRepository(MultibancaDBContext context) : IRegistroContactoRepository
{
    /// <inheritdoc />
    public async Task<List<registro_contacto_bbva>> GetByExpediente(long idExpediente)
    {
        List<registro_contacto_bbva> registros = await context.RegistroContactoBbva
            .Where(q =>
                q.id_expediente == idExpediente &&
                q.is_active &&
                q.row_status)
            .OrderByDescending(q => q.fecha_contacto)
            .ThenByDescending(q => q.nro_contacto ?? 0)
            .ThenByDescending(q => q.id)
            .ToListAsync();

        return registros;
    }

    /// <inheritdoc />
    public async Task<registro_contacto_bbva> Crear(registro_contacto_bbva request, int userId)
    {
        int siguienteNumero = await ObtenerSiguienteNumero(request.id_expediente);

        request.nro_contacto = siguienteNumero;
        request.id_usuario = userId;
        request.is_active = true;
        request.row_status = true;
        request.created_by = userId;
        request.created_date = DateTime.Now;
        request.modified_by = null;
        request.modified_date = null;

        context.RegistroContactoBbva.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    /// <summary>
    /// Calcula el siguiente consecutivo visible del historico por expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente.</param>
    /// <returns>Siguiente numero de contacto.</returns>
    private async Task<int> ObtenerSiguienteNumero(long idExpediente)
    {
        int? ultimoNumero = await context.RegistroContactoBbva
            .Where(q =>
                q.id_expediente == idExpediente &&
                q.is_active &&
                q.row_status)
            .MaxAsync(q => q.nro_contacto);

        return (ultimoNumero ?? 0) + 1;
    }
}
