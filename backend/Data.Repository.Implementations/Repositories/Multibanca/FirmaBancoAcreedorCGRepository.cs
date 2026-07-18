using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class FirmaBancoAcreedorCGRepository
        : MultibancaGenericRepository<firma_banco_acreedor_cg_entity>,
          IFirmaBancoAcreedorCGRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public FirmaBancoAcreedorCGRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<firma_banco_acreedor_cg_entity?> GetByExpediente(
            long id_expediente
        )
        {
            var data = new { p_id_expediente = id_expediente };

            DbConnection connection =
                this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText =
                    "SELECT * FROM usp_select_firma_banco_acreedor_cg(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<firma_banco_acreedor_cg_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Registrar Firma Banco Acreedor CG del expediente {id_expediente}.",
                    ex
                );
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
