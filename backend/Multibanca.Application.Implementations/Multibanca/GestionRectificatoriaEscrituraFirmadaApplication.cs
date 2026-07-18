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
    public class GestionRectificatoriaEscrituraFirmadaApplication : MultibancaGenericApplication<gestion_rectificatoria_escritura_firmada, gestion_rectificatoria_escritura_firmada_entity, IGestionRectificatoriaEscrituraFirmadaRepository>, IGestionRectificatoriaEscrituraFirmadaApplication
    {
        private readonly IGestionRectificatoriaEscrituraFirmadaRepository GestionRectificatoriaEscrituraFirmadaProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IRectificatoriaFirmaApplication RectificatoriaFirmaApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public GestionRectificatoriaEscrituraFirmadaApplication(
            MultibancaDBContext _multibancaDBContext,
            IGestionRectificatoriaEscrituraFirmadaRepository _gestionRectificatoriaEscrituraFirmadaRepository,
            IWorkflowApplication _workflowApplicationProvider,
            IRectificatoriaFirmaApplication _rectificatoriaFirmaApplicationProvider,
            IUserApplication _userApplicationProvider,
            IMapper _mapper)
            : base(_multibancaDBContext, _gestionRectificatoriaEscrituraFirmadaRepository, _mapper)
        {
            GestionRectificatoriaEscrituraFirmadaProvider = _gestionRectificatoriaEscrituraFirmadaRepository;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            RectificatoriaFirmaApplicationProvider = _rectificatoriaFirmaApplicationProvider;
            UserApplicationProvider = _userApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Gestion Rectificatoria Escritura Firmada para el expediente {expediente_id}.");
            }

            string envioTipoReparo = entity.enviar_tipo_reparo ?? "";

            string transitionsID;

            string transitionName = envioTipoReparo switch
            {
                "01" => "GestionRectificatoriaEscrituraFirmada_GestionRectificatoriaSolucionReparo_COMERCIAL",
                "02" => "ReparoLegal",
                "03" => "GestionRectificatoriaEscrituraFirmada_RectificatoriaFirmaCompradorVendedor_NOTARIA",
                "" => "GestionRectificatoriaEscrituraFirmada_RectificatoriaLegalCartaResguardo",
                _ => string.Empty
            };

            if (transitionName == null)
            {
                throw new InvalidOperationException($"No existe registro de transitionName para el expediente {expediente_id}.");
            }

            if (transitionName == "ReparoLegal")
            {
                throw new InvalidOperationException($"El expediente {expediente_id} se encuentra en Reparo Legal.");
            }

            transitionsID = listTransitions
                                .Where(x => x.name == transitionName)
                                .Select(x => x.transition_id)
                                .FirstOrDefault() ?? string.Empty;

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }

        public async Task<gestion_rectificatoria_escritura_firmada?> GetByExpediente(long id_expediente)
        {
            // Obtener la entidad principal
            var entity = await GestionRectificatoriaEscrituraFirmadaProvider.GetByExpediente(id_expediente);

            if (entity == null)
                return null;

            // Obtener datos desde RectificatoriaFirma (tabla padre)
            var rectificatoriaFirma = await RectificatoriaFirmaApplicationProvider.GetByExpediente(id_expediente);

            // Mapear la entidad principal
            var result = Mapper.Map<gestion_rectificatoria_escritura_firmada?>(entity);

            if (result != null && rectificatoriaFirma != null)
            {
                var user = UserApplicationProvider.FindId(rectificatoriaFirma.created_by);
                result.id_solicitud = rectificatoriaFirma.id_rectificatoria_firma;
                result.id_solicitante = rectificatoriaFirma.created_by;
                result.solicitante = user != null
                    ? $"{user?.name} {user?.last_name_first} {user?.last_name_second}".Trim()
                    : rectificatoriaFirma.created_by.ToString();
                result.observaciones_reparo = rectificatoriaFirma.observaciones;
                result.fecha_ingreso = rectificatoriaFirma.created_date;
            }

            return result;
        }
    }
}