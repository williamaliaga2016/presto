using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

/// <summary>
/// Repositorio EF Core para la persistencia de la confirmacion de soportes de pago.
/// </summary>
public class CargarSoportesPagoRepository : ICargarSoportesPagoRepository
{
    private readonly MultibancaDBContext MultibancaDBContext;

    /// <summary>
    /// Inicializa el repositorio con el contexto principal de BBVA Legalizacion.
    /// </summary>
    /// <param name="multibancaDBContext">Contexto EF Core de `BBVA_LEGALIZACION`.</param>
    public CargarSoportesPagoRepository(MultibancaDBContext multibancaDBContext)
    {
        MultibancaDBContext = multibancaDBContext;
    }

    /// <summary>
    /// Obtiene el ultimo registro activo de confirmacion asociado al expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Soportes de Pago o `null` si no existe.</returns>
    public async Task<cargar_soportes_pago_entity?> GetByExpediente(long idExpediente)
    {
        return await MultibancaDBContext.CargarSoportesPago
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Crea o actualiza el registro activo de confirmacion de soportes de pago del expediente.
    /// </summary>
    /// <param name="request">Datos de confirmacion y observaciones capturados en Cargar Soportes de Pago.</param>
    /// <param name="userId">Usuario autenticado que realiza la operacion.</param>
    /// <returns>Registro creado o actualizado.</returns>
    public async Task<cargar_soportes_pago_entity> Guardar(
        cargar_soportes_pago_entity request,
        int userId)
    {
        cargar_soportes_pago_entity? existing = await MultibancaDBContext.CargarSoportesPago
            .FirstOrDefaultAsync(q => q.id_expediente == request.id_expediente && q.is_active && q.row_status);

        if (existing == null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            MultibancaDBContext.CargarSoportesPago.Add(request);
            await MultibancaDBContext.SaveChangesAsync();
            return request;
        }

        existing.documentos_adjuntos = request.documentos_adjuntos;
        existing.observaciones = request.observaciones;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        await MultibancaDBContext.SaveChangesAsync();
        return existing;
    }
}
