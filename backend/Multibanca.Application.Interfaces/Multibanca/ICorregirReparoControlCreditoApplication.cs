using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICorregirReparoControlCreditoApplication : IMultibancaGenericApplication<corregir_reparo_control_credito>
    {
        Task<corregir_reparo_control_credito?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
