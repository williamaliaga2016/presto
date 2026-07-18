using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RevisarDesembolsoRepository : MultibancaGenericRepository<revisar_desembolso_entity>, IRevisarDesembolsoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;
        public RevisarDesembolsoRepository(MultibancaDBContext multibancaDBContext) : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }
        public async Task<revisar_desembolso_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_revisar_desembolso(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<revisar_desembolso_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Revisar Desembolso del expediente {id_expediente}. Detalle: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
