using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.CargaOperacionBanco
{
    public class CargaOperacionBancoAntecedenteCompradorRepository :
        MultibancaGenericRepository<carga_operacion_banco_antecedente_comprador_entity>,
        ICargaOperacionBancoAntecedenteCompradorRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public CargaOperacionBancoAntecedenteCompradorRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<carga_operacion_banco_antecedente_comprador_entity>> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_carga_operacion_banco_antecedente_comprador(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToListDomain<carga_operacion_banco_antecedente_comprador_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Antecedentes del Comprador del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
