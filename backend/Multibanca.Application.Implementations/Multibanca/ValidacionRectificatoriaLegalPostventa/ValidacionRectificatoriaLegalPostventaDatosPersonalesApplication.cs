using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Multibanca.Application.Implementations.Multibanca.ValidacionRectificatoriaLegalPostventa
{
    public class ValidacionRectificatoriaLegalPostventaDatosPersonalesApplication :
        MultibancaGenericApplication<
            validacion_rectificatoria_legal_postventa_datos_personales,
            validacion_rectificatoria_legal_postventa_datos_personales_entity,
            IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository>,
        IValidacionRectificatoriaLegalPostventaDatosPersonalesApplication
    {
        private readonly IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider;
        private readonly IMapper Mapper;

        public ValidacionRectificatoriaLegalPostventaDatosPersonalesApplication(
            MultibancaDBContext _multibancaDBContext,
            IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository,
            IMapper _mapper)
            : base(_multibancaDBContext, _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository, _mapper)
        {
            ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider = _validacionRectificatoriaLegalRepositoryDatosPersonalesRepository;
            Mapper = _mapper;
        }

        public async Task<List<validacion_rectificatoria_legal_postventa_datos_personales>> GetByExpediente(long id_expediente)
        {
            List<validacion_rectificatoria_legal_postventa_datos_personales_entity> entity =
                await ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider.GetByExpediente(id_expediente);

            return Mapper.Map<List<validacion_rectificatoria_legal_postventa_datos_personales>>(entity);
        }
    }
}
