using Common.Application.Interfaces;
using Data.Repository.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura
{
    public interface IGenerarBorradorEscrituraApplication : IMultibancaGenericApplication<generar_borrador_escritura>
    {
        Task<generar_borrador_escritura> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
