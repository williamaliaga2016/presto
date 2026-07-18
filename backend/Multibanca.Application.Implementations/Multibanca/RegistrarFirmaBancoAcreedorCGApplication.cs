using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RegistrarFirmaBancoAcreedorCGApplication
        : MultibancaGenericApplication<
              firma_banco_acreedor_cg,
              firma_banco_acreedor_cg_entity,
              IFirmaBancoAcreedorCGRepository
          >,
          IRegistrarFirmaBancoAcreedorCGApplication
    {
        private readonly IFirmaBancoAcreedorCGRepository FirmaBancoAcreedorCGRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaBancoAcreedorCGApplication(
            MultibancaDBContext _multibancaDBContext,
            IFirmaBancoAcreedorCGRepository _firmaBancoAcreedorCGRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _firmaBancoAcreedorCGRepository, _mapper)
        {
            FirmaBancoAcreedorCGRepositoryProvider = _firmaBancoAcreedorCGRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<firma_banco_acreedor_cg?> GetByExpediente(long id_expediente)
        {
            var entity = await FirmaBancoAcreedorCGRepositoryProvider.GetByExpediente(
                id_expediente
            );
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<firma_banco_acreedor_cg?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await FirmaBancoAcreedorCGRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Registrar Firma Banco Acreedor CG para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "RegistrarFirmaBancoAcreedorCG_RealizarRevisionPrevioFirmaBanco")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Registrar Firma Banco Acreedor CG."
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
