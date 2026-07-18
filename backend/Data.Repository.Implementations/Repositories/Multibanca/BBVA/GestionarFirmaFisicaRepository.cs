using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

public class GestionarFirmaFisicaRepository(MultibancaDBContext multibancaDBContext) : IGestionarFirmaFisicaRepository
{
    public async Task<gestionar_firma_fisica_bbva?> GetByExpediente(long idExpediente)
    {
        return await multibancaDBContext.GestionarFirmaFisicaBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    public async Task<gestionar_firma_fisica_bbva> Guardar(gestionar_firma_fisica_bbva request, int userId)
    {
        var existing = await multibancaDBContext.GestionarFirmaFisicaBbva
            .FirstOrDefaultAsync(q => q.id_expediente == request.id_expediente && q.is_active && q.row_status);

        if (existing == null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            multibancaDBContext.GestionarFirmaFisicaBbva.Add(request);
            await multibancaDBContext.SaveChangesAsync();
            return request;
        }

        existing.id_actividad = request.id_actividad;
        existing.motorizado_asignado = request.motorizado_asignado;
        existing.fecha_gestoria = request.fecha_gestoria;
        existing.resultado_gestoria = request.resultado_gestoria;
        existing.observaciones = request.observaciones;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        await multibancaDBContext.SaveChangesAsync();
        return existing;
    }
}
