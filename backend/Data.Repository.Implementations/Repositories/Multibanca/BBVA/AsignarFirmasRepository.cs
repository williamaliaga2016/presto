using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

public class AsignarFirmasRepository(MultibancaDBContext context) : IAsignarFirmasRepository
{
    public Task<asignar_firmas_peritos_abogados?> GetByExpediente(long idExpediente) =>
        context.AsignarFirmasBbva
            .Where(x => x.id_expediente == idExpediente && x.is_active && x.row_status)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

    public async Task<asignar_firmas_peritos_abogados> Guardar(
        asignar_firmas_peritos_abogados request, int userId)
    {
        var existing = await GetByExpediente(request.id_expediente);
        if (existing is null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            context.AsignarFirmasBbva.Add(request);
            await context.SaveChangesAsync();
            return request;
        }

        existing.tipo_cliente = request.tipo_cliente;
        existing.codigo_ejecutivo_solicitante = request.codigo_ejecutivo_solicitante;
        existing.oficina_solicitante = request.oficina_solicitante;
        existing.tipo_solicitud_avaluo = request.tipo_solicitud_avaluo;
        existing.tipo_tramite_eett = request.tipo_tramite_eett;
        existing.nombre_firma_supervisor = request.nombre_firma_supervisor;
        existing.telefono_firma = request.telefono_firma;
        existing.email_firma = request.email_firma;
        existing.valor_avaluo = request.valor_avaluo;
        existing.valor_total_consignar = request.valor_total_consignar;
        existing.opciones_recaudo = request.opciones_recaudo;
        existing.numero_recaudo = request.numero_recaudo;
        existing.banco = request.banco;
        existing.nombre_abogado = request.nombre_abogado;
        existing.telefono_abogado = request.telefono_abogado;
        existing.valor_estudio_titulos = request.valor_estudio_titulos;
        existing.tipo_cuenta_abogado = request.tipo_cuenta_abogado;
        existing.numero_cuenta_abogado = request.numero_cuenta_abogado;
        existing.requiere_envio_notificacion = request.requiere_envio_notificacion;
        existing.checklist_documentos_solicitar = request.checklist_documentos_solicitar;
        existing.observaciones = request.observaciones;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;
        await context.SaveChangesAsync();
        return existing;
    }
}

