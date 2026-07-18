using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;

namespace Multibanca.Application.Implementations.Multibanca.GenerarBorradorEscritura
{
    public class GenerarBorradorEscrituraApplication : MultibancaGenericApplication<generar_borrador_escritura, generar_borrador_escritura_entity, IGenerarBorradorEscrituraRepository>, IGenerarBorradorEscrituraApplication
    {
        private readonly IGenerarBorradorEscrituraDetalleApplication DetalleApplicationProvider;
        private readonly IGenerarBorradorEscrituraRepository RepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GenerarBorradorEscrituraApplication(MultibancaDBContext _multibancaDBContext, IGenerarBorradorEscrituraDetalleApplication _detalleApplicationProvider, IGenerarBorradorEscrituraRepository _repository, IWorkflowApplication workflowApplicationProvider, IMapper _mapper) : base(_multibancaDBContext, _repository, _mapper)
        {
            DetalleApplicationProvider = _detalleApplicationProvider;
            RepositoryProvider = _repository;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await RepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null) {
                throw new InvalidOperationException($"No existe registro de Generar Borrador Escritura para el expediente {expediente_id}.");
            }
            var folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            var listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool envioReparo = entity.enviar_reparo;

            string transitionsID;
            if (!envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "GenerarBorradorEscritura_EndEvent")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;

            }
            else 
            { 
                transitionsID= listTransitions
                    .Where(x=>x.name == "GenerarBorradorEscritura_CorregirReparoBorradorEscritura_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }


                return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        public async Task<generar_borrador_escritura> GetByExpediente(long id_expediente)
        {
            var entity =
                await RepositoryProvider.GetByExpediente(id_expediente);

            if (entity == null)
                return null;

            var model =
                Mapper.Map<generar_borrador_escritura>(entity);

            model.detalle =
                await DetalleApplicationProvider.GetList(
                    model.id_generar_borrador_escritura,
                    model.id_expediente
                );

            return model;
        }
    }
}