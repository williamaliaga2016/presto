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
    public class GestionRectificatoriaApplication : MultibancaGenericApplication<gestion_rectificatoria, gestion_rectificatoria_entity, IGestionRectificatoriaRepository>, IGestionRectificatoriaApplication
    {
        private readonly IGestionRectificatoriaRepository GestionRectificatoriaRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GestionRectificatoriaApplication(MultibancaDBContext multibancaDBContext, IGestionRectificatoriaRepository generarPreFiniquitoRepositoryProvider, IWorkflowApplication workflowApplicationProvider, IMapper mapper) : base(multibancaDBContext, generarPreFiniquitoRepositoryProvider, mapper)
        {
            GestionRectificatoriaRepositoryProvider = generarPreFiniquitoRepositoryProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = mapper;
        }
        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await GestionRectificatoriaRepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Gestión Rectificatoria para el expediente {expediente_id}."
                );
            }

            string transitionsID = entity.enviar_tipo_reparo switch
            {
                "01" => listTransitions
                    .FirstOrDefault(x => x.name == "GestiónRectificatoria_GestionRectificatoriaSoluciónReparo_ReparoComercial")?
                    .transition_id ?? string.Empty,
                "02" => listTransitions
                    .FirstOrDefault(x => x.name == "GestiónRectificatoria_ValidaciónRectificatoriaLegal_ReparoLegal")?
                    .transition_id ?? string.Empty,
                "03" => listTransitions
                    .FirstOrDefault(x => x.name == "GestiónRectificatoria_CorregirControlEscritura_ReparoForma")?
                    .transition_id ?? string.Empty,
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    $"No se encontró transición configurada para el tipo de reparo '{entity.enviar_tipo_reparo}' en Gestión Rectificatoria."
                );
            }

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();
            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }


        public async Task<gestion_rectificatoria?> GetByExpediente(long id_expediente)
        {
            var entity = await GestionRectificatoriaRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<gestion_rectificatoria?>(entity);
        }
    }
}
