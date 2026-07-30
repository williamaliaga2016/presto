using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

public interface ITradicionesConocidasRepository
{
    Task<tradiciones_conocidas_entity?> GetByCodigoProyecto(string codigoProyecto);
}
