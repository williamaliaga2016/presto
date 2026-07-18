using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Domain.Models.FuncTransversal;

namespace Multibanca.Application.Implementations.FuncTransversal
{
    public class BitacoraApplication :
        MultibancaGenericApplication<bitacora, bitacora_entity, IBitacoraRepository>,
        IBitacoraApplication
    {
        private readonly IBitacoraRepository BitacoraRepositoryProvider;
        private readonly IMapper Mapper;

        public BitacoraApplication(
            MultibancaDBContext _multibancaDBContext,
            IBitacoraRepository _bitacoraRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _bitacoraRepository, _mapper)
        {
            BitacoraRepositoryProvider = _bitacoraRepository;
            Mapper = _mapper;
        }

        public async Task<bitacora?> GetByExpedienteActividad(long id_expediente, string id_actividad)
        {
            bitacora_entity? entity =
                await BitacoraRepositoryProvider.GetByExpedienteActividad(id_expediente, id_actividad);

            return Mapper.Map<bitacora?>(entity);
        }
    }
}