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
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class AprobacionComercialLegalCdRApplication : MultibancaGenericApplication<aprobacion_comercial_legal_cdr, aprobacion_comercial_legal_cdr_entity, IAprobacionComercialLegalCdRRepository>, IAprobacionComercialLegalCdRApplication
    {
        private readonly IAprobacionComercialLegalCdRRepository AprobacionRepository;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public AprobacionComercialLegalCdRApplication(
            MultibancaDBContext _multibancaDBContext,
            IAprobacionComercialLegalCdRRepository _aprobacionRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _aprobacionRepository, _mapper)
        {
            AprobacionRepository = _aprobacionRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<aprobacion_comercial_legal_cdr?> GetByExpediente(long id_expediente)
        {
            var entity = await AprobacionRepository.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<aprobacion_comercial_legal_cdr?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await AprobacionRepository.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Realizar Aprobación Comercial / Legal CdR para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);

            string transitionsID;
            if (entity.enviar_a_reparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarAprobacionComercialLegalCdr_CorregirReparoCdr_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarAprobacionComercialLegalCdr_RegistrarFirmaBancoAcreedor_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

    }
}
