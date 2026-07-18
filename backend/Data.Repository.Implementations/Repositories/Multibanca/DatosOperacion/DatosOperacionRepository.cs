using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.DatosOperacion
{
    public class DatosOperacionRepository :
        MultibancaGenericRepository<datos_operacion_entity>,
        IDatosOperacionRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public DatosOperacionRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<datos_operacion_entity> GetByExpediente(long id_expediente)
        {
            var data = new
            {
                p_id_expediente = id_expediente
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_datos_operacion(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<datos_operacion_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos_operacion del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
