using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ReparoEstudioTitulosDetalleRepository : MultibancaGenericRepository<reparo_estudio_titulos_detalle_entity>, IReparoEstudioTitulosDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ReparoEstudioTitulosDetalleRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<reparo_estudio_titulos_detalle_entity> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_reparo_estudio_titulos_detalle(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<reparo_estudio_titulos_detalle_entity>() ?? new reparo_estudio_titulos_detalle_entity();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Reparo Estudio Títulos Detalle del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
