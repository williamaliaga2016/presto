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
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GestionRectificatoriaEscrituraFirmadaPostventaApplication : MultibancaGenericApplication<gestion_rectificatoria_escritura_firmada_postventa, gestion_rectificatoria_escritura_firmada_postventa_entity, IGestionRectificatoriaEscrituraFirmadaPostventaRepository>, IGestionRectificatoriaEscrituraFirmadaPostventaApplication
    {
        private readonly IGestionRectificatoriaEscrituraFirmadaPostventaRepository GestionRectificatoriaEscrituraFirmadaPostventaProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IRectificatoriaAnalisisDerivacionReparoPostventaApplication RectificatoriaFirmaApplicationIRectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public GestionRectificatoriaEscrituraFirmadaPostventaApplication(
            MultibancaDBContext _multibancaDBContext,
            IGestionRectificatoriaEscrituraFirmadaPostventaRepository _gestionRectificatoriaEscrituraFirmadaPostventaRepository,
            IWorkflowApplication _workflowApplicationProvider,
            IRectificatoriaAnalisisDerivacionReparoPostventaApplication _rectificatoria_analisis_derivacion_reparo_postventaApplicationIRectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider,
            IUserApplication _userApplicationProvider,
            IMapper _mapper)
            : base(_multibancaDBContext, _gestionRectificatoriaEscrituraFirmadaPostventaRepository, _mapper)
        {
            GestionRectificatoriaEscrituraFirmadaPostventaProvider = _gestionRectificatoriaEscrituraFirmadaPostventaRepository;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            RectificatoriaFirmaApplicationIRectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider = _rectificatoria_analisis_derivacion_reparo_postventaApplicationIRectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider;
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
                throw new InvalidOperationException($"No existe registro de Gestion Rectificatoria Escritura Firmada Postventa para el expediente {expediente_id}.");
            }

            string envioTipoReparo = entity.enviar_tipo_reparo ?? "";

            string transitionsID;

            string transitionName = envioTipoReparo switch
            {
                "01" => "GestionRectificatoriaEscrituraFirmadaPostventa_RectificatoriaAnalisisDerivaciónReparoPostventa_COMERCIAL",
                "02" => "ReparoLegal",
                "03" => "GestionRectificatoriaEscrituraFirmadaPostventa_RectificatoriaFirmaCompradorVendedorPostventa_NOTARIA",
                "" => "GestionRectificatoriaEscrituraFirmadaPostventa_RectificatoriaLegalCierreCopiasPostventa",
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

            transitionsID = listTransitions.Select(q => q.transition_id).FirstOrDefault() ?? "";
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }

        public async Task<gestion_rectificatoria_escritura_firmada_postventa?> GetByExpediente(long id_expediente)
        {
            var entity = await GestionRectificatoriaEscrituraFirmadaPostventaProvider.GetByExpediente(id_expediente);

            if (entity == null)
                return null;

            var rectificatoria_analisis_derivacion_reparo_postventa = await RectificatoriaFirmaApplicationIRectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.GetByExpediente(id_expediente);

            var result = Mapper.Map<gestion_rectificatoria_escritura_firmada_postventa?>(entity);

            if (result != null && rectificatoria_analisis_derivacion_reparo_postventa != null)
            {
                var user = UserApplicationProvider.FindId(rectificatoria_analisis_derivacion_reparo_postventa.created_by);
                result.id_solicitud = rectificatoria_analisis_derivacion_reparo_postventa.id_rectificatoria_analisis_derivacion_reparo_postventa;
                result.id_solicitante = rectificatoria_analisis_derivacion_reparo_postventa.created_by;
                result.solicitante = user != null
                    ? $"{user?.name} {user?.last_name_first} {user?.last_name_second}".Trim()
                    : rectificatoria_analisis_derivacion_reparo_postventa.created_by.ToString();
                result.observaciones_reparo = rectificatoria_analisis_derivacion_reparo_postventa.observaciones;
                result.fecha_ingreso = rectificatoria_analisis_derivacion_reparo_postventa.created_date;
            }

            return result;
        }
    }
}
