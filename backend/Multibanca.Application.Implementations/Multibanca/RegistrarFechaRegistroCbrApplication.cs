using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RegistrarFechaRegistroCbrApplication : MultibancaGenericApplication<registrar_fecha_registro_cbr, registrar_fecha_registro_cbr_entity, IRegistrarFechaRegistroCbrRepository>, IRegistrarFechaRegistroCbrApplication
    {
        private readonly IRegistrarFechaRegistroCbrRepository RegistrarFechaRegistroCbrRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFechaRegistroCbrApplication(
            MultibancaDBContext _multibancaDBContext,
            IRegistrarFechaRegistroCbrRepository _registrarFechaRegistroCbrRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _registrarFechaRegistroCbrRepository, _mapper)
        {
            RegistrarFechaRegistroCbrRepositoryProvider = _registrarFechaRegistroCbrRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<registrar_fecha_registro_cbr?> GetByExpediente(long id_expediente)
        {
            var entity = await RegistrarFechaRegistroCbrRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<registrar_fecha_registro_cbr?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await RegistrarFechaRegistroCbrRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Registrar Fecha Registro CBR para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "RegistrarFechaRegistroCBR_RevisarInscripcionCBR")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Registrar Fecha Registro CBR."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }
    }
}
