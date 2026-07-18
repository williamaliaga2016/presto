using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ReingresarEscrituraCbrRepository
        : MultibancaGenericRepository<reingresar_escritura_cbr_entity>,
          IReingresarEscrituraCbrRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ReingresarEscrituraCbrRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<reingresar_escritura_cbr_entity?> GetByExpediente(
            long id_expediente
        )
        {
            var data = new
            {
                p_id_expediente = id_expediente
            };

            DbConnection connection = MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                command.CommandText = @"
                    SELECT *
                    FROM public.usp_select_reingresar_escritura_cbr(
                        CAST(@p_id_expediente AS bigint)
                    );
                ";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToDomain<reingresar_escritura_cbr_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Reingresar Escritura en CBR del expediente {id_expediente}.",
                    ex
                );
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
