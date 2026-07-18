using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class GenerarFiniquitoRepository : MultibancaGenericRepository<generar_finiquito_entity>, IGenerarFiniquitoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public GenerarFiniquitoRepository(MultibancaDBContext multibancaDBContext) : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<generar_finiquito_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_generar_finiquito(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<generar_finiquito_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Generar Finiquito del expediente {id_expediente}. Detalle: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
