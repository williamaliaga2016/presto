using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class CorregirReparoPrefiniquitoRepository
        : MultibancaGenericRepository<corregir_reparo_prefiniquito_entity>,
          ICorregirReparoPrefiniquitoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public CorregirReparoPrefiniquitoRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<corregir_reparo_prefiniquito_entity?> GetByExpediente(
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
                    FROM public.usp_select_corregir_reparo_prefiniquito(
                        @p_id_expediente
                    );
                ";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToDomain<corregir_reparo_prefiniquito_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Corregir Reparo Prefiniquito del expediente {id_expediente}.",
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
