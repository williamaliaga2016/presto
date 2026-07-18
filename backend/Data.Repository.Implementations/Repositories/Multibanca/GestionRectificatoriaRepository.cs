using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
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
    public class GestionRectificatoriaRepository : MultibancaGenericRepository<gestion_rectificatoria_entity>, IGestionRectificatoriaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public GestionRectificatoriaRepository(MultibancaDBContext multibancaDBContext) : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<gestion_rectificatoria_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_gestion_rectificatoria(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<gestion_rectificatoria_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Generar PreFiniquito del expediente {id_expediente}. Detalle: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
