using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class AprobacionComercialLegalCdRRepository : MultibancaGenericRepository<aprobacion_comercial_legal_cdr_entity>, IAprobacionComercialLegalCdRRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public AprobacionComercialLegalCdRRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<aprobacion_comercial_legal_cdr_entity?> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_aprobacion_comercial_legal_cdr(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<aprobacion_comercial_legal_cdr_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Aprobacion Comercial Legal CdR del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
