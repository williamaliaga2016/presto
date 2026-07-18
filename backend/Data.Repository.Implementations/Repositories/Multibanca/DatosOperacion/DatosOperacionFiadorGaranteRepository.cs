using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.DatosOperacion
{
    public class DatosOperacionFiadorGaranteRepository :
        MultibancaGenericRepository<datos_operacion_fiador_garante_entity>,
        IDatosOperacionFiadorGaranteRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public DatosOperacionFiadorGaranteRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<datos_operacion_fiador_garante_entity>> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_datos_operacion_fiador_garante(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<datos_operacion_fiador_garante_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos_operacion_fiador_garante del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
