using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.FuncTransversal
{
    public interface IHistorialExpedienteRepository
    {
        Task<List<HistorialExpedienteDTO>> CargaInicialHistorial(long idExpediente);
    }
}
