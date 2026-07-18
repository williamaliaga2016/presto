using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CorregirReparoDatosOperacionApplication
        : MultibancaGenericApplication<corregir_reparo_datos_operacion, corregir_reparo_datos_operacion_entity, ICorregirReparoDatosOperacionRepository>,
          ICorregirReparoDatosOperacionApplication
    {
        private readonly ICorregirReparoDatosOperacionRepository CorregirReparoDatosOperacionRepositoryProvider;
        private readonly IDatosOperacionApplication DatosOperacionApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoDatosOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoDatosOperacionRepository _corregirReparoDatosOperacionRepository,
            IDatosOperacionApplication _datosOperacionApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoDatosOperacionRepository, _mapper)
        {
            CorregirReparoDatosOperacionRepositoryProvider = _corregirReparoDatosOperacionRepository;
            DatosOperacionApplicationProvider = _datosOperacionApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_datos_operacion?> GetByExpediente(long id_expediente)
        {
            var entity =
                await CorregirReparoDatosOperacionRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_datos_operacion?>(entity)
                ?? new corregir_reparo_datos_operacion();

            domain.id_expediente = id_expediente;

            datos_operacion? datosOperacion = null;

            try
            {
                datosOperacion = await DatosOperacionApplicationProvider.GetByExpediente(id_expediente);
            }
            catch
            {
                datosOperacion = null;
            }

            if (datosOperacion != null)
            {
                domain.observaciones_reparo = datosOperacion.observaciones;
                domain.fecha_ingreso = datosOperacion.modified_date ?? datosOperacion.created_date;

                if (domain.id_usuario_solicitante == 0)
                {
                    domain.id_usuario_solicitante = datosOperacion.modified_by ?? datosOperacion.created_by;
                }
            }

            domain.solicitante = GetNombreSolicitante(domain.id_usuario_solicitante);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await CorregirReparoDatosOperacionRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Datos Operación para el expediente {expediente_id}."
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

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo Datos Operación."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private string GetNombreSolicitante(int id_usuario_solicitante)
        {
            if (id_usuario_solicitante <= 0)
            {
                return string.Empty;
            }

            try
            {
                users user = UserApplicationProvider.FindId(id_usuario_solicitante);

                if (user == null)
                {
                    return string.Empty;
                }

                return string.Join(
                    " ",
                    new[]
                    {
                        user.name,
                        user.last_name_first,
                        user.last_name_second
                    }.Where(x => !string.IsNullOrWhiteSpace(x))
                ).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
