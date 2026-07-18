using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

public class MockAsignadorFirmasService : IAsignadorFirmasService
{
    public Task<object> Calcular(CalcularAsignacionRequest request)
    {
        object result = new
        {
            nombre_firma_supervisor = "Avalúos Colombia S.A.S. / Laura Martínez",
            telefono_firma = "+57 601 555 0148",
            email_firma = "asignaciones@avaluoscolombia.test",
            valor_avaluo = 650000m,
            valor_total_consignar = 650000m,
            opciones_recaudo = "Transferencia o PSE",
            numero_recaudo = $"AV-{request.id_expediente}",
            banco = "BBVA Colombia",
            nombre_abogado = "Carlos Andrés Rodríguez",
            telefono_abogado = "+57 300 555 0192",
            valor_estudio_titulos = 480000m,
            tipo_cuenta_abogado = "Ahorros",
            numero_cuenta_abogado = "001300012345"
        };
        return Task.FromResult(result);
    }
}

