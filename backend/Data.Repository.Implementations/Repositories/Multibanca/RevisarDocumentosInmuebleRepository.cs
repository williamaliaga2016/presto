using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RevisarDocumentosInmuebleRepository
        : MultibancaGenericRepository<revisar_documentos_inmueble_entity>,
          IRevisarDocumentosInmuebleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisarDocumentosInmuebleRepository(MultibancaDBContext multibancaDBContext)
            : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<revisar_documentos_inmueble_entity?> GetByExpediente(long idExpediente)
        {
            var data = new
            {
                p_id_expediente = idExpediente
            };

            DbConnection connection = MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = @"  SELECT * FROM usp_select_revisar_inmueble(@p_id_expediente);";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToDomain<revisar_documentos_inmueble_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Revisar Documentos Inmueble del expediente {idExpediente}. Detalle: {ex.InnerException?.Message ?? ex.Message}",
                    ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
