using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICalculoGeneracionDocumentoApplication : IMultibancaGenericApplication<calculo_generacion_documento>
    {
        Task<calculo_generacion_documento?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
        Task<decimal?> CalcularUF(DateTime fecha);
    }
}
