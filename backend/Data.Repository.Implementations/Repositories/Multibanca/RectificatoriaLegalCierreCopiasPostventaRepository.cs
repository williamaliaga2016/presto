using System.Data;
using System.Data.Common;
using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RectificatoriaLegalCierreCopiasPostventaRepository
        : MultibancaGenericRepository<rectificatoria_legal_cierre_copias_postventa_entity>,
            IRectificatoriaLegalCierreCopiasPostventaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RectificatoriaLegalCierreCopiasPostventaRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<rectificatoria_legal_cierre_copias_postventa_entity?> GetByExpediente(long id_expediente)
        {
            var data = new { p_id_expediente = id_expediente };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText =
                    "SELECT * FROM usp_select_rectificatoria_legal_cierre_copias_postventa(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<rectificatoria_legal_cierre_copias_postventa_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Rectificatoria Legal Cierre de Copias Postventa del expediente {id_expediente}.",
                    ex
                );
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
