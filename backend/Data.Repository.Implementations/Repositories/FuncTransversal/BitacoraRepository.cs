using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.FuncTransversal
{
    public class BitacoraRepository : MultibancaGenericRepository<bitacora_entity>, IBitacoraRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public BitacoraRepository(MultibancaDBContext _multibancaDBContext)
            : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<bitacora_entity> GetByExpedienteActividad(long id_expediente, string id_actividad)
        {
            object data = new
            {
                p_id_expediente = id_expediente,
                p_id_actividad = id_actividad
            };

            bitacora_entity bitacora = new bitacora_entity();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT *
                FROM usp_select_bitacora_by_expediente_actividad(
                    @p_id_expediente,
                    @p_id_actividad
                );
            ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    bitacora = reader.MapToDomain<bitacora_entity>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }
            }

            return bitacora;
        }
    }
}