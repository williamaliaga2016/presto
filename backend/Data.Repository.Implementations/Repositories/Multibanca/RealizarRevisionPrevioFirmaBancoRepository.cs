using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RealizarRevisionPrevioFirmaBancoRepository : MultibancaGenericRepository<realizar_revision_previo_firma_banco_entity>, IRealizarRevisionPrevioFirmaBancoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarRevisionPrevioFirmaBancoRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_revision_previo_firma_banco_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_realizar_revision_previo_firma_banco(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<realizar_revision_previo_firma_banco_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Realizar Revisión Previo Firma Banco del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
