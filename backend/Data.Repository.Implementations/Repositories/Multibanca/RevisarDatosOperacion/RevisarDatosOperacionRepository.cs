using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionRepository :
        MultibancaGenericRepository<revisar_datos_operacion_entity>,
        IRevisarDatosOperacionRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarDatosOperacionRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<revisar_datos_operacion_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_revisar_datos_operacion(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<revisar_datos_operacion_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Revisar Datos Operación del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
