using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ReparoVisadoDetalleRepository : IReparoVisadoDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ReparoVisadoDetalleRepository(MultibancaDBContext _multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<reparo_visado_detalle_entity> GetByExpediente(long id_expediente)
        {
            var data = new { p_id_expediente = id_expediente };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_reparo_visado_detalle(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<reparo_visado_detalle_entity>() ?? new reparo_visado_detalle_entity();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Reparo Visado Detalle del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task MarkSubsanado(int id_visar_operacion, int userId)
        {
            var data = new
            {
                p_id_visar_operacion = id_visar_operacion,
                p_modified_by = userId,
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText =
                    @"UPDATE public.visar_operacion
                         SET is_active     = false,
                             row_status    = false,
                             modified_by   = @p_modified_by,
                             modified_date = now()
                       WHERE id_visar_operacion = @p_id_visar_operacion;";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar el reparo {id_visar_operacion} como subsanado.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
