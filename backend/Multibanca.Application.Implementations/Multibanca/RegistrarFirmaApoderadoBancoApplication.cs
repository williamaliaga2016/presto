using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
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
    public class RegistrarFirmaApoderadoBancoApplication : MultibancaGenericApplication<registrar_firma_apoderado_banco, registrar_firma_apoderado_banco_entity, IRegistrarFirmaApoderadoBancoRepository>, IRegistrarFirmaApoderadoBancoApplication
    {
        private readonly IRegistrarFirmaApoderadoBancoRepository RegistrarFirmaApoderadoBancoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaApoderadoBancoApplication(MultibancaDBContext multibancaDBContext, IRegistrarFirmaApoderadoBancoRepository generarPreFiniquitoRepositoryProvider, IWorkflowApplication workflowApplicationProvider, IMapper mapper) : base(multibancaDBContext, generarPreFiniquitoRepositoryProvider, mapper)
        {
            RegistrarFirmaApoderadoBancoRepositoryProvider = generarPreFiniquitoRepositoryProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await RegistrarFirmaApoderadoBancoRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Registrar Firma Apoderado Banco para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "RegistrarFirmaApoderadoBanco_RealizarCierreCopiasNotaria")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Registrar Firma Apoderado Banco."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }


        public async Task<registrar_firma_apoderado_banco?> GetByExpediente(long id_expediente)
        {
            var entity = await RegistrarFirmaApoderadoBancoRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<registrar_firma_apoderado_banco?>(entity);
        }
    }
}
