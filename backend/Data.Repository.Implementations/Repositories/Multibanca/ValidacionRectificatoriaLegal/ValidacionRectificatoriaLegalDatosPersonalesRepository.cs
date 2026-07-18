using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;


namespace Data.Repository.Implementations.Repositories.Multibanca.ValidacionRectificatoriaLegal
{
    public class ValidacionRectificatoriaLegalDatosPersonalesRepository : MultibancaGenericRepository<validacion_rectificatoria_legal_datos_personales_entity>, IValidacionRectificatoriaLegalDatosPersonalesRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ValidacionRectificatoriaLegalDatosPersonalesRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<validacion_rectificatoria_legal_datos_personales_entity>> GetByExpediente(long id_expediente)
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
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_validacion_rectificatoria_legal_datos_personales(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<validacion_rectificatoria_legal_datos_personales_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener información del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
