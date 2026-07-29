using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion;

public class TradicionesConocidasRepository : ITradicionesConocidasRepository
{
    private readonly MultibancaDBContext _dbContext;

    public TradicionesConocidasRepository(MultibancaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<tradiciones_conocidas_entity?> GetByCodigoProyecto(string codigoProyecto)
    {
        DbConnection connection = _dbContext.Database.GetDbConnection();
        await using var command = connection.CreateCommand();

        try
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            command.CommandText = "SELECT * FROM usp_select_tradiciones_conocidas_by_codigo_proyecto(@p_codigo_proyecto);";
            command.CommandType = CommandType.Text;

            var param = command.CreateParameter();
            param.ParameterName = "p_codigo_proyecto";
            param.Value = codigoProyecto;
            command.Parameters.Add(param);

            await using var reader = await command.ExecuteReaderAsync();
            return reader.MapToDomain<tradiciones_conocidas_entity>();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }
}
