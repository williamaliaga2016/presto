using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class VerificarCorreccionEscrituraApplication : MultibancaGenericApplication<verificar_correccion_escritura, verificar_correccion_escritura_entity, IVerificarCorreccionEscrituraRepository>, IVerificarCorreccionEscrituraApplication
    {
        private readonly IVerificarCorreccionEscrituraRepository VerificarCorreccionEscrituraRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public VerificarCorreccionEscrituraApplication(MultibancaDBContext _multibancaDBContext, IVerificarCorreccionEscrituraRepository _verificarCorreccionEscrituraRepositoryProvider, IWorkflowApplication _workflowApplicationProvider, IMapper _mapper) : base(_multibancaDBContext, _verificarCorreccionEscrituraRepositoryProvider, _mapper)
        {
            VerificarCorreccionEscrituraRepositoryProvider = _verificarCorreccionEscrituraRepositoryProvider;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await VerificarCorreccionEscrituraRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Verificar Corrección de Escritura para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "VerificarCorreccionEscritura_RegistrarFirmaApoderadoBanco")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Verificar Corrección de Escritura."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }


        public async Task<verificar_correccion_escritura?> GetByExpediente(long id_expediente)
        {
            var entity = await VerificarCorreccionEscrituraRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<verificar_correccion_escritura?>(entity);
        }
    }
}
