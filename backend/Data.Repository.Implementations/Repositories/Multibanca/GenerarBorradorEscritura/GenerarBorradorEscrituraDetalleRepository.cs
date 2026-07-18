using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca.GenerarBorradorEscritura
{
    public class GenerarBorradorEscrituraDetalleRepository : MultibancaGenericRepository<generar_borrador_escritura_detalle_entity>, IGenerarBorradorEscrituraDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public GenerarBorradorEscrituraDetalleRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

  
        public async Task<List<generar_borrador_escritura_detalle_entity>> GetList(int id_generar_borrador_escritura,long id_expediente)
        {
            var data = new
            {
                p_id_generar_borrador_escritura = id_generar_borrador_escritura,
                p_id_expediente = id_expediente
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();

            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText =
                    "SELECT * FROM usp_select_generar_borrador_escritura_detalle( @p_id_generar_borrador_escritura,@p_id_expediente);";

                command.CommandType = CommandType.Text;

                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToListDomain<generar_borrador_escritura_detalle_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener el detalle de Generar Borrador Escritura. " +
                    $"Cabecera: {id_generar_borrador_escritura}, " +
                    $"Expediente: {id_expediente}.",
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
