using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.FuncTransversal
{
    public interface IHistorialExpedienteApplication
    {
        Task<List<HistorialExpedienteDTO>> CargaInicialHistorial(long idExpediente);
    }
}
