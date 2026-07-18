using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CorregirReparoControlCreditoApplication : MultibancaGenericApplication<corregir_reparo_control_credito, corregir_reparo_control_credito_entity, ICorregirReparoControlCreditoRepository>, ICorregirReparoControlCreditoApplication
    {
        private readonly ICorregirReparoControlCreditoRepository CorregirReparoControlCreditoProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IRealizarControlCreditoApplication RealizarControlCreditoApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoControlCreditoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoControlCreditoRepository _corregirReparoControlCreditoRepository,
            IWorkflowApplication _workflowApplicationProvider,
            IRealizarControlCreditoApplication _realizarControlCreditoApplicationProvider,
            IUserApplication _userApplicationProvider,
            IMapper _mapper)
            : base(_multibancaDBContext, _corregirReparoControlCreditoRepository, _mapper)
        {
            CorregirReparoControlCreditoProvider = _corregirReparoControlCreditoRepository;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            RealizarControlCreditoApplicationProvider = _realizarControlCreditoApplicationProvider;
            UserApplicationProvider = _userApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await CorregirReparoControlCreditoProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Control de Crédito para el expediente {expediente_id}."
                );
            }

            if (!entity.subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "CorregirReparoControlCredito_RealizarControlCredito")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo Control de Crédito."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }


        public async Task<corregir_reparo_control_credito?> GetByExpediente(long id_expediente)
        {
            var entity = await CorregirReparoControlCreditoProvider.GetByExpediente(id_expediente);

            if (entity == null)
                return null;

            var realizarControlCredito = await RealizarControlCreditoApplicationProvider.GetByExpediente(id_expediente);

            var result = Mapper.Map<corregir_reparo_control_credito?>(entity);

            if (result != null && realizarControlCredito != null)
            {
                var user = UserApplicationProvider.FindId(realizarControlCredito.created_by);
                result.id_solicitud = realizarControlCredito.id_realizar_control_credito;
                result.id_solicitante = realizarControlCredito.created_by;
                result.solicitante = user != null
                    ? $"{user.name} {user.last_name_first} {user.last_name_second}".Trim()
                    : realizarControlCredito.created_by.ToString();
                result.observaciones_reparo = realizarControlCredito.observaciones;
                result.fecha_ingreso = realizarControlCredito.created_date;
            }

            return result;
        }
    }
}