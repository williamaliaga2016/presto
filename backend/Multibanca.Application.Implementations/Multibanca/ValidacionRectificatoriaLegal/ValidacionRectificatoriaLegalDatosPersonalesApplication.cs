using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;

namespace Multibanca.Application.Implementations.Multibanca.ValidacionRectificatoriaLegal
{
    public class ValidacionRectificatoriaLegalDatosPersonalesApplication :
        MultibancaGenericApplication<
            validacion_rectificatoria_legal_datos_personales,
            validacion_rectificatoria_legal_datos_personales_entity,
            IValidacionRectificatoriaLegalDatosPersonalesRepository>,
        IValidacionRectificatoriaLegalDatosPersonalesApplication
    {
        private readonly IValidacionRectificatoriaLegalDatosPersonalesRepository ValidacionRectificatoriaLegalDatosPersonalesRepositoryProvider;
        private readonly IMapper Mapper;

        public ValidacionRectificatoriaLegalDatosPersonalesApplication(
            MultibancaDBContext _multibancaDBContext,
            IValidacionRectificatoriaLegalDatosPersonalesRepository _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository, _mapper)
        {
            ValidacionRectificatoriaLegalDatosPersonalesRepositoryProvider = _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository;
            Mapper = _mapper;
        }

        public async Task<List<validacion_rectificatoria_legal_datos_personales>> GetByExpediente(long id_expediente)
        {
            List<validacion_rectificatoria_legal_datos_personales_entity> entity =
                await ValidacionRectificatoriaLegalDatosPersonalesRepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<List<validacion_rectificatoria_legal_datos_personales>>(entity);
        }
    }
}
