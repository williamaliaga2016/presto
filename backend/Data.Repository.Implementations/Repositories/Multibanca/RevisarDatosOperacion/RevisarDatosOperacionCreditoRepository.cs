using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionCreditoRepository :
        MultibancaGenericRepository<revisar_datos_operacion_credito_entity>,
        IRevisarDatosOperacionCreditoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarDatosOperacionCreditoRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<revisar_datos_operacion_credito_entity> GetByExpediente(long id_expediente)
        {
            return await GetByExpediente(id_expediente, 0);
        }

        public async Task<revisar_datos_operacion_credito_entity> GetByExpediente(long id_expediente, int id_revisar_datos_operacion)
        {
            var data = new
            {
                p_id_expediente = id_expediente,
                p_id_revisar_datos_operacion = id_revisar_datos_operacion
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_revisar_datos_operacion_credito(@p_id_expediente, @p_id_revisar_datos_operacion);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<revisar_datos_operacion_credito_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener revisar_datos_operacion_credito del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
