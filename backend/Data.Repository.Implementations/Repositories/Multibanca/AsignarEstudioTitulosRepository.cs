using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class AsignarEstudioTitulosRepository : MultibancaGenericRepository<asignar_estudio_titulos_entity>, IAsignarEstudioTitulosRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public AsignarEstudioTitulosRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<asignar_estudio_titulos_entity?> GetByExpedienteActividad(long id_expediente, string id_actividad)
        {
            var data = new
            {
                p_id_expediente = id_expediente,
                p_id_actividad = id_actividad
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_asignar_estudio_titulos(@p_id_expediente, @p_id_actividad);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.HasRows ? reader.MapToDomain<asignar_estudio_titulos_entity>() : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Asignar Estudio de Títulos del expediente {id_expediente} y actividad {id_actividad}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}

