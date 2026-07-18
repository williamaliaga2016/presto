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
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RectificatoriaPostventaSolucionReparoApplication
        : MultibancaGenericApplication<rectificatoria_postventa_solucion_reparo, rectificatoria_postventa_solucion_reparo_entity, IRectificatoriaPostventaSolucionReparoRepository>,
          IRectificatoriaPostventaSolucionReparoApplication
    {
        private readonly IRectificatoriaPostventaSolucionReparoRepository RectificatoriaPostventaSolucionReparoRepositoryProvider;
        private readonly IRectificatoriaAnalisisDerivacionReparoPostventaApplication RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaPostventaSolucionReparoApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaPostventaSolucionReparoRepository _repository,
            IRectificatoriaAnalisisDerivacionReparoPostventaApplication _rectificatoriaAnalisisDerivacionReparoPostventaApplication,
            IWorkflowApplication _workflowApplication,
            IUserApplication _userApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _repository, _mapper)
        {
            RectificatoriaPostventaSolucionReparoRepositoryProvider = _repository;
            RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider = _rectificatoriaAnalisisDerivacionReparoPostventaApplication;
            WorkflowApplicationProvider = _workflowApplication;
            UserApplicationProvider = _userApplication;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_postventa_solucion_reparo?> GetByExpediente(
            long id_expediente
        )
        {
            var entity =
                await RectificatoriaPostventaSolucionReparoRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<rectificatoria_postventa_solucion_reparo?>(entity)
                ?? new rectificatoria_postventa_solucion_reparo();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await RectificatoriaPostventaSolucionReparoRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Rectificatoria PostVenta Solucion Reparo para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
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

            string targetTransitionName="";

            switch (entity.modificar_datos_memo, entity.descontabilizar_operacion)
            {
                case (true, false):
                    targetTransitionName = "RectificatoriaAnalisisDerivacionReparoPostventaSolucionReparo_ValidacionRectificatoriaLegalPostventa";
                    break;

                case (false, true):
                    targetTransitionName = "RectificatoriaAnalisisDerivacionReparoPostventaSolucionReparo_EndEvent";
                    break;

                case (false, false):
                    throw new InvalidOperationException(
                            "Debe marcar estatus de la actividad antes de avanzar."
                        );

                case (true, true):
                    throw new InvalidOperationException(
                        "No puede marcar ambos estatus antes de avanzar."
                    );
            }

            string transitionsID = listTransitions
                .Where(x => x.name == targetTransitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Rectificatoria PostVenta Solucion Reparo."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            rectificatoria_postventa_solucion_reparo domain,
            long id_expediente
        )
        {
            rectificatoria_analisis_derivacion_reparo_postventa? actividadOrigen =
                await RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.GetByExpediente(
                    id_expediente
                );

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante <= 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                users user = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                if (user != null)
                {
                    domain.solicitante = string.Join(
                        " ",
                        new[]
                        {
                            user.name,
                            user.last_name_first,
                            user.last_name_second
                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                    );
                }
            }
        }
    }
}
