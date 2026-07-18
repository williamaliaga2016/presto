using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionFiadorGaranteRepository :
        MultibancaGenericRepository<revisar_datos_operacion_fiador_garante_entity>,
        IRevisarDatosOperacionFiadorGaranteRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarDatosOperacionFiadorGaranteRepository(MultibancaDBContext multibancaDBContext)
            : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<List<revisar_datos_operacion_fiador_garante_entity>> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_revisar_datos_operacion_fiador_garante(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<revisar_datos_operacion_fiador_garante_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener revisar_datos_operacion_fiador_garante del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
