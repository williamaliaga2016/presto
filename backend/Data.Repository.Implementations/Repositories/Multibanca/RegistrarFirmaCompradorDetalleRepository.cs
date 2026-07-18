using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RegistrarFirmaCompradorDetalleRepository : MultibancaGenericRepository<firma_comprador_detalle_entity>, IRegistrarFirmaCompradorDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RegistrarFirmaCompradorDetalleRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<firma_comprador_detalle_entity>> GetByIdFirmaComprador(int id_firma_comprador)
        {
            var data = new { p_id_firma_comprador = id_firma_comprador };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_firma_comprador_detalle(@p_id_firma_comprador);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.HasRows ? reader.MapToListDomain<firma_comprador_detalle_entity>() : new List<firma_comprador_detalle_entity>(); ;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Firma Comprador detalle con el id {id_firma_comprador}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<firma_comprador_detalle_entity>> GetDefaultFromExpediente(long id_expediente)
        {
            var data = new { p_id_expediente = id_expediente };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            command.CommandText = "SELECT * FROM usp_select_default_firma_comprador_detalle(@p_id_expediente);";
            command.CommandType = CommandType.Text;
            command.Parameters.ToArray(data);

            await using var reader = await command.ExecuteReaderAsync();

            return reader.HasRows
                ? reader.MapToListDomain<firma_comprador_detalle_entity>()
                : new List<firma_comprador_detalle_entity>();
        }
    }
}
