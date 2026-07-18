using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.FuncTransversal
{
    public class HistorialExpedienteApplication : IHistorialExpedienteApplication
    {
        private readonly IHistorialExpedienteRepository HistorialExpedienteRepositoryProvider;

        public HistorialExpedienteApplication(IHistorialExpedienteRepository _historialExpedienteRepository)
        {
            HistorialExpedienteRepositoryProvider = _historialExpedienteRepository;
        }

        public async Task<List<HistorialExpedienteDTO>> CargaInicialHistorial(long idExpediente)
        {
            return await HistorialExpedienteRepositoryProvider.CargaInicialHistorial(idExpediente);
        }
    }
}
