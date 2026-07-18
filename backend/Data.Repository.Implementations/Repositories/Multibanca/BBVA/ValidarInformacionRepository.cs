using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

public class ValidarInformacionRepository : IValidarInformacionRepository
{
    private readonly MultibancaDBContext MultibancaDBContext;

    public ValidarInformacionRepository(MultibancaDBContext multibancaDBContext)
    {
        MultibancaDBContext = multibancaDBContext;
    }

    public async Task<validar_informacion_bbva?> GetByExpediente(long idExpediente)
    {
        return await MultibancaDBContext.ValidarInformacionBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    public async Task<validar_informacion_bbva> Guardar(validar_informacion_bbva request, int userId)
    {
        var existing = await MultibancaDBContext.ValidarInformacionBbva
            .FirstOrDefaultAsync(q => q.id_expediente == request.id_expediente && q.is_active && q.row_status);

        if (existing == null)
        {
            request.is_active = true;
            request.row_status = true;
            request.created_by = userId;
            request.created_date = DateTime.Now;
            MultibancaDBContext.ValidarInformacionBbva.Add(request);
            await MultibancaDBContext.SaveChangesAsync();
            return request;
        }
        CopyEditableFields(request, existing);
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        await MultibancaDBContext.SaveChangesAsync();
        return existing;
    }

    public async Task<IReadOnlyList<titular_bbva>> GetTitulares(long idExpediente)
    {
        return await MultibancaDBContext.TitularBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderBy(q => q.numero_titular)
            .ThenBy(q => q.id)
            .ToListAsync();
    }

    public async Task<titular_bbva> AgregarTitular(titular_bbva request, int userId)
    {
        int titularesActivos = await MultibancaDBContext.TitularBbva
            .CountAsync(q => q.id_expediente == request.id_expediente && q.is_active && q.row_status);

        if (titularesActivos >= 10)
            throw new InvalidOperationException("Limite maximo de titulares alcanzado");

        request.numero_titular = titularesActivos + 1;
        request.is_active = true;
        request.row_status = true;
        request.created_by = userId;
        request.created_date = DateTime.Now;

        MultibancaDBContext.TitularBbva.Add(request);
        await SincronizarTitularEnFormulario(request, userId);
        await MultibancaDBContext.SaveChangesAsync();
        return request;
    }

    public async Task<bool> ActualizarCondicionesFinancieras(validar_informacion_bbva request, int userId)
    {
        if (request == null) return false;

        var existing = await MultibancaDBContext.ValidarInformacionBbva
            .FirstOrDefaultAsync(x =>
                x.id_expediente == request.id_expediente &&
                x.is_active &&
                x.row_status);

        if (existing == null) return false;

        existing.monto_otorgado_vi = request.monto_otorgado_vi;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        return true;
    }

    public async Task<bool> ActualizarDatosCredito(validar_informacion_bbva request, int userId)
    {
        if (request == null) return false;

        var existing = await MultibancaDBContext.ValidarInformacionBbva
            .FirstOrDefaultAsync(x =>
                x.id_expediente == request.id_expediente &&
                x.is_active &&
                x.row_status);

        if (existing == null) return false;

        existing.tipo_credito = request.tipo_credito;
        existing.tiene_garantia = request.tiene_garantia ?? false;
        existing.modified_by = userId;
        existing.modified_date = DateTime.Now;

        return true;
    }

    private static void CopyEditableFields(validar_informacion_bbva source, validar_informacion_bbva target)
    {
        target.tipo_id_t1 = source.tipo_id_t1;
        target.numero_id_t1 = source.numero_id_t1;
        target.nombre_completo_t1 = source.nombre_completo_t1;
        target.celular_t1 = source.celular_t1;
        target.telefono_t1 = source.telefono_t1;
        target.email_t1 = source.email_t1;
        target.direccion_t1 = source.direccion_t1;
        target.departamento_t1 = source.departamento_t1;
        target.municipio_t1 = source.municipio_t1;
        target.situacion_laboral_t1 = source.situacion_laboral_t1;
        target.cliente_nomina_t1 = source.cliente_nomina_t1;
        target.tipo_id_t2 = source.tipo_id_t2;
        target.numero_id_t2 = source.numero_id_t2;
        target.nombre_completo_t2 = source.nombre_completo_t2;
        target.celular_t2 = source.celular_t2;
        target.email_t2 = source.email_t2;
        target.tipo_id_t3 = source.tipo_id_t3;
        target.numero_id_t3 = source.numero_id_t3;
        target.nombre_completo_t3 = source.nombre_completo_t3;
        target.celular_t3 = source.celular_t3;
        target.email_t3 = source.email_t3;
        target.inmueble_definido = source.inmueble_definido;
        target.tipo_inmueble = source.tipo_inmueble;
        target.estado_inmueble = source.estado_inmueble;
        target.constructora = source.constructora;
        target.es_constructora_vip = source.es_constructora_vip;
        target.codigo_proyecto = source.codigo_proyecto;
        target.descripcion_proyecto = source.descripcion_proyecto;
        target.departamento_inmueble = source.departamento_inmueble;
        target.municipio_inmueble = source.municipio_inmueble;
        target.fecha_estimada_entrega = source.fecha_estimada_entrega;
        target.estatus_general = source.estatus_general;
        target.origen_devolucion = source.origen_devolucion;
        target.requiere_definir_inmueble = source.requiere_definir_inmueble;
        target.requiere_carga_cliente = source.requiere_carga_cliente;
        target.requiere_carga_constructora = source.requiere_carga_constructora;
        target.tipo_credito = source.tipo_credito;
        target.tiene_garantia = source.tiene_garantia;
        target.garantia_constituida = source.garantia_constituida;
        target.monto_otorgado_vi = source.monto_otorgado_vi;
        target.monto_otorgado_vivienda_original = source.monto_otorgado_vivienda_original;
        target.correo_declarativo = source.correo_declarativo;
        target.telefono_declarativo = source.telefono_declarativo;
        target.codigo_oficina = source.codigo_oficina;
        target.descripcion_oficina = source.descripcion_oficina;
        target.codigo_asesor = source.codigo_asesor;
        target.motivo_devolucion = source.motivo_devolucion;
        target.observaciones = source.observaciones;
    }

    private async Task SincronizarTitularEnFormulario(titular_bbva titular, int userId)
    {
        if (titular.numero_titular < 1 || titular.numero_titular > 3) return;

        var formulario = await MultibancaDBContext.ValidarInformacionBbva
            .FirstOrDefaultAsync(q =>
                q.id_expediente == titular.id_expediente &&
                q.is_active &&
                q.row_status);

        if (formulario == null)
        {
            formulario = new validar_informacion_bbva
            {
                id_expediente = titular.id_expediente,
                id_actividad = titular.id_actividad ?? "ACT_VALIDAR_INFO",
                is_active = true,
                row_status = true,
                created_by = userId,
                created_date = DateTime.Now
            };
            MultibancaDBContext.ValidarInformacionBbva.Add(formulario);
        }

        switch (titular.numero_titular)
        {
            case 1:
                formulario.tipo_id_t1 = titular.tipo_identificacion;
                formulario.numero_id_t1 = titular.numero_identificacion;
                formulario.nombre_completo_t1 = titular.nombre_completo;
                formulario.celular_t1 = titular.celular_cliente;
                formulario.telefono_t1 = titular.telefono_residente;
                formulario.email_t1 = titular.email;
                formulario.direccion_t1 = titular.direccion_residencia;
                formulario.telefono_declarativo = titular.telefono_declarativo ?? formulario.telefono_declarativo;
                formulario.correo_declarativo = titular.correo_declarativo ?? formulario.correo_declarativo;
                break;
            case 2:
                formulario.tipo_id_t2 = titular.tipo_identificacion;
                formulario.numero_id_t2 = titular.numero_identificacion;
                formulario.nombre_completo_t2 = titular.nombre_completo;
                formulario.celular_t2 = titular.celular_cliente;
                formulario.email_t2 = titular.email;
                break;
            case 3:
                formulario.tipo_id_t3 = titular.tipo_identificacion;
                formulario.numero_id_t3 = titular.numero_identificacion;
                formulario.nombre_completo_t3 = titular.nombre_completo;
                formulario.celular_t3 = titular.celular_cliente;
                formulario.email_t3 = titular.email;
                break;
        }

        formulario.modified_by = userId;
        formulario.modified_date = DateTime.Now;
    }
}
