using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GestionReparoApplication : MultibancaGenericApplication<gestion_reparo, gestion_reparo_entity, IGestionReparoRepository>, IGestionReparoApplication
    {
        private readonly IGestionReparoRepository GestionReparoProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IGestionRectificatoriaApplication GestionRectificatoriaApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public GestionReparoApplication(
            MultibancaDBContext _multibancaDBContext,
            IGestionReparoRepository _corregirReparoControlCreditoRepository,
            IWorkflowApplication _workflowApplicationProvider,
            IGestionRectificatoriaApplication _gestionRectificatoriaApplicationProvider,
            IUserApplication _userApplicationProvider,
            IMapper _mapper)
            : base(_multibancaDBContext, _corregirReparoControlCreditoRepository, _mapper)
        {
            GestionReparoProvider = _corregirReparoControlCreditoRepository;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            GestionRectificatoriaApplicationProvider = _gestionRectificatoriaApplicationProvider;
            UserApplicationProvider = _userApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await GestionReparoProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Gestion de Reparo para el expediente {expediente_id}."
                );
            }

            if (!entity.subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);


            string transitionsID = listTransitions.Select(q => q.transition_id).FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Gestion de Reparo por Gestor."
                );
            }

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }

        public async Task<gestion_reparo?> GetByExpediente(long id_expediente)
        {
            var entity = await GestionReparoProvider.GetByExpediente(id_expediente);

            if (entity == null)
                return null;

            var gestionRectificatoria = await GestionRectificatoriaApplicationProvider.GetByExpediente(id_expediente);

            var result = Mapper.Map<gestion_reparo?>(entity);

            if (result != null && gestionRectificatoria != null)
            {
                var user = UserApplicationProvider.FindId(gestionRectificatoria.created_by);
                result.id_solicitud = gestionRectificatoria.id_gestion_rectificatoria;
                result.id_solicitante = user.user_id;
                result.solicitante = user != null
            ? $"{user?.name} {user?.last_name_first} {user?.last_name_second}".Trim()
            : gestionRectificatoria.created_by.ToString();
                result.observaciones_reparo = gestionRectificatoria.observaciones;
                result.fecha_ingreso = gestionRectificatoria.created_date;
            }

            return result;
        }
    }
}