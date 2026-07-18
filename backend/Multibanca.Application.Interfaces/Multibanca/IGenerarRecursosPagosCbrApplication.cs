using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IGenerarRecursosPagosCbrApplication : IMultibancaGenericApplication<generar_recursos_pagos_cbr>
    {
        Task<generar_recursos_pagos_cbr?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
