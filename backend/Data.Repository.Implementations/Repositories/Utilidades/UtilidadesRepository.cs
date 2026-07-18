using System.Data;
using System.Data.Common;
using Data.Extensions.Repository;
using Data.Repository.Interfaces.Repositories.Utilidades;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Common;
using Multibanca.DTO.Utilidades;

namespace Data.Repository.Implementations.Repositories.Utilidades
{
    public class UtilidadesRepository : IUtilidadesRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public UtilidadesRepository(MultibancaDBContext multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<int> Validate_RequestNumber(long idExpediente)
        {
            var data = new
            {
                p_id_expediente = idExpediente
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_get_count_activity_by_id_expediente(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return reader["result"] != null && reader["result"] != DBNull.Value
                        ? int.Parse(reader["result"].ToString()!)
                        : 0;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al validar actividades vigentes del expediente {idExpediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<ControlBaseDTO>> GetActivitiesUsers(long idExpediente)
        {
            var data = new
            {
                p_id_expediente = idExpediente
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_get_activity_background(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<ControlBaseDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener actividades anteriores del expediente {idExpediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<ControlBaseDTO>> GetUsersActivities(long idExpediente, long idActividad)
        {
            var data = new
            {
                p_id_expediente = idExpediente,
                p_id_actividad = idActividad
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_get_users_reassign(@p_id_expediente, @p_id_actividad);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<ControlBaseDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuarios para reasignación del expediente {idExpediente}, actividad {idActividad}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<bool> Save_DeleteReassign(UtilidadesDTO util, int idUsuario)
        {
            var data = new
            {
                p_id_expediente = util.id_expediente,
                p_option = util.accion_id,
                p_user_id = idUsuario,
                p_user_id_assign = util.actividad_usuario_id,
                p_id_activity = util.actividad_id
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_get_user_activity_list(@p_id_expediente, @p_option, @p_user_id, @p_user_id_assign, @p_id_activity);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    if (reader["res"] != null && reader["res"] != DBNull.Value)
                        return Convert.ToBoolean(reader["res"]);

                    if (reader["result"] != null && reader["result"] != DBNull.Value)
                        return Convert.ToBoolean(reader["result"]);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar utilidad para el expediente {util.id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<ReassignmentWithdrawalDTO> GetMessageUtility(
            int opcion,
            long idExpediente,
            int idUsuario,
            int? idUsuarioReasignado,
            long? idActividad)
        {
            var data = new
            {
                p_opcion = opcion,
                p_id_expediente = idExpediente,
                p_id_usuario = idUsuario,
                p_id_usuario_reasignado = idUsuarioReasignado,
                p_id_actividad = idActividad
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_get_message_utility(@p_opcion, @p_id_expediente, @p_id_usuario, @p_id_usuario_reasignado, @p_id_actividad);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<ReassignmentWithdrawalDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener mensaje de utilidad para el expediente {idExpediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}