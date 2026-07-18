using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

public class GestionarFirmaRepository(MultibancaDBContext multibancaDBContext) : IGestionarFirmaRepository
{
    public async Task<gestionar_firma_bbva?> GetByExpediente(long idExpediente)
    {
        return await multibancaDBContext.GestionarFirmaBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    public async Task<gestionar_firma_bbva> Guardar(gestionar_firma_bbva request, int userId)
    {
        var existing = await multibancaDBContext.GestionarFirmaBbva
            .FirstOrDefaultAsync(q => q.id_expediente == request.id_expediente && q.is_active && q.row_status);

        if (existing == null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            multibancaDBContext.GestionarFirmaBbva.Add(request);
            await multibancaDBContext.SaveChangesAsync();
            return request;
        }

        existing.id_actividad = request.id_actividad;
        existing.requiere_firma_electronica = request.requiere_firma_electronica;
        existing.firma_electronica_realizada = request.firma_electronica_realizada;
        existing.nombre_cliente_firma = request.nombre_cliente_firma;
        existing.nombre_solicitante_firma = request.nombre_solicitante_firma;
        existing.franja_horaria = request.franja_horaria;
        existing.direccion_firma = request.direccion_firma;
        existing.descripcion_tramite = request.descripcion_tramite;
        existing.fecha_programacion = request.fecha_programacion;
        existing.ciudad_cliente = request.ciudad_cliente;
        existing.tipo_credito_firma = request.tipo_credito_firma;
        existing.observaciones = request.observaciones;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        await multibancaDBContext.SaveChangesAsync();
        return existing;
    }
}