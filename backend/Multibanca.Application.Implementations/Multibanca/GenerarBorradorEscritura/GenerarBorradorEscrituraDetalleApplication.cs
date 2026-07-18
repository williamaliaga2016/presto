using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca.GenerarBorradorEscritura
{
    public class GenerarBorradorEscrituraDetalleApplication: MultibancaGenericApplication<generar_borrador_escritura_detalle, generar_borrador_escritura_detalle_entity, IGenerarBorradorEscrituraDetalleRepository>, IGenerarBorradorEscrituraDetalleApplication
    {
        private readonly IGenerarBorradorEscrituraDetalleRepository RepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GenerarBorradorEscrituraDetalleApplication(MultibancaDBContext _multibancaDBContext, IGenerarBorradorEscrituraDetalleRepository _repository, IWorkflowApplication workflowApplicationProvider, IMapper _mapper) : base(_multibancaDBContext, _repository, _mapper)
        {

            RepositoryProvider = _repository;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<generar_borrador_escritura_detalle>> GetList(int id_generar_borrador_escritura, long id_expediente)
        {
            var entities = await RepositoryProvider.GetList(id_generar_borrador_escritura,id_expediente);

            return Mapper.Map<List<generar_borrador_escritura_detalle>>(entities);
        }
    }
}
