using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ActividadesRepository : MultibancaGenericRepository<actividades_entity>, IActividadesRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ActividadesRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<actividades_entity> ObtenerInformacionActividadPorId(long id)
        {
            var data = new
            {
                p_id = id
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_wf_infoactividad_id(@p_id);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<actividades_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener información de la actividad {id}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<actividades_entity>> GetCurrentActivitiesByIdExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_get_current_activities_by_id_expediente(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<actividades_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener actividades vigentes del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<bool> CompletarActividad(long id, long id_usuario)
        {
            var data = new
            {
                p_id = id,
                p_id_usuario = id_usuario
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT usp_update_wf_completar_actividad(@p_id, @p_id_usuario);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                var result = await command.ExecuteScalarAsync();

                return result != null
                       && result != DBNull.Value
                       && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al completar la actividad {id}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<bool> ExisteActividad(long idExpediente, string idActividad)
        {
            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT usp_select_wf_existe_actividad(@p_id_expediente, @p_id_actividad);";
                command.CommandType = CommandType.Text;

                var param1 = command.CreateParameter();
                param1.ParameterName = "p_id_expediente";
                param1.Value = idExpediente;
                command.Parameters.Add(param1);

                var param2 = command.CreateParameter();
                param2.ParameterName = "p_id_actividad";
                param2.Value = idActividad;
                command.Parameters.Add(param2);

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return false;

                var existe = Convert.ToInt32(result);

                if(existe == 0) { 
                    return false;
                }
                else if(existe == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Valor inesperado retornado por la función usp_select_wf_existe_actividad: {existe}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al validar si existe la actividad '{idActividad}' para el expediente {idExpediente}. Detalle: {ex.Message}",
                    ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<long> InsertActividad(actividades_entity actividadEntity)
        {
            var connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = @"
                SELECT usp_insertar_wf_actividad(
                    p_id := @p_id::bigint,
                    p_id_expediente := @p_id_expediente::bigint,
                    p_id_actividad := @p_id_actividad::varchar(40),
                    p_id_rol := @p_id_rol::bigint,
                    p_status := @p_status::varchar(40),
                    p_id_usuario := @p_id_usuario::integer,
                    p_descripcion := @p_descripcion::varchar(150),
                    p_activo := @p_activo::boolean,
                    p_fecha_alta := @p_fecha_alta::timestamp,
                    p_fecha_asignacion := @p_fecha_asignacion::timestamp,
                    p_fecha_inicio := @p_fecha_inicio::timestamp,
                    p_fecha_termino := @p_fecha_termino::timestamp,
                    p_fecha_cancelacion := @p_fecha_cancelacion::timestamp,
                    p_fecha_actualizacion := @p_fecha_actualizacion::timestamp,
                    p_fecha_reingreso := @p_fecha_reingreso::timestamp,
                    p_fecha_suspencion := @p_fecha_suspencion::timestamp,
                    p_fecha_reactivacion := @p_fecha_reactivacion::timestamp
                );";

                command.CommandType = CommandType.Text;

                var data = new
                {
                    p_id = actividadEntity.id,
                    p_id_expediente = actividadEntity.id_expediente,
                    p_id_actividad = actividadEntity.id_actividad,
                    p_id_rol = actividadEntity.id_rol,
                    p_status = actividadEntity.status,
                    p_id_usuario = actividadEntity.id_usuario,
                    p_descripcion = actividadEntity.descripcion,
                    p_activo = actividadEntity.activo,
                    p_fecha_alta = actividadEntity.fecha_alta,
                    p_fecha_asignacion = actividadEntity.fecha_asignacion,
                    p_fecha_inicio = actividadEntity.fecha_inicio,
                    p_fecha_termino = actividadEntity.fecha_termino,
                    p_fecha_cancelacion = actividadEntity.fecha_cancelacion,
                    p_fecha_actualizacion = actividadEntity.fecha_actualizacion,
                    p_fecha_reingreso = actividadEntity.fecha_reingreso,
                    p_fecha_suspencion = actividadEntity.fecha_suspencion,
                    p_fecha_reactivacion = actividadEntity.fecha_reactivacion
                };

                command.Parameters.ToArray(data);

                var result = await command.ExecuteScalarAsync();

                return result != null && result != DBNull.Value
                    ? Convert.ToInt64(result)
                    : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar o actualizar la actividad.", ex);
            }
        }

        public async Task<bool> ValidaEditActividad(long id_expediente, int id_usuario, string activity_id)
        {
            var data = new
            {
                p_id_expediente = id_expediente,
                p_id_usuario = id_usuario,
                p_activity_id = activity_id
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT usp_is_edit_actividad(@p_id_expediente, @p_id_usuario, @p_activity_id);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                var result = await command.ExecuteScalarAsync();

                return result != null
                       && result != DBNull.Value
                       && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al validar edición de actividad para expediente {id_expediente}, usuario {id_usuario}, actividad {activity_id}.",
                    ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<bool> IsCompleteActivity(long id_expediente, string activity_id)
        {
            var data = new
            {
                p_id_expediente = id_expediente,
                p_activity_id = activity_id
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT usp_is_complete_activity(@p_id_expediente, @p_activity_id);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                var result = await command.ExecuteScalarAsync();

                return result != null
                       && result != DBNull.Value
                       && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al validar si la actividad '{activity_id}' del expediente {id_expediente} está completada.",
                    ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<actividades_entity> ObtenerActividadPorExpedienteActividad(long id_expediente, string id_actividad)
        {
            return await MultibancaDBContext.Set<actividades_entity>()
                .AsNoTracking()
                .Where(x => x.id_expediente == id_expediente && x.id_actividad == id_actividad)
                .OrderByDescending(x => x.id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verifica si existe una instancia PENDIENTE (Nueva / En progreso) de la actividad.
        /// Evita que reintentos del flujo CreateCase+AvanzarActividad creen duplicados de una
        /// actividad que ya está pendiente, PERO permite recrear una actividad que fue
        /// completada en una iteración anterior de un loop de reparo (Verificar &lt;-&gt; Corregir).
        /// No basta con filtrar activo = true: 'activo' es borrado lógico, no el estado;
        /// una actividad Completada sigue con activo = true y bloquearía el reingreso al loop.
        /// </summary>
        public async Task<bool> ExisteActividadActiva(long id_expediente, string id_actividad)
        {
            return await MultibancaDBContext.Set<actividades_entity>()
                .AsNoTracking()
                .AnyAsync(x => x.id_expediente == id_expediente
                               && x.id_actividad == id_actividad
                               && x.activo == true
                               && (x.status == "Nueva" || x.status == "En progreso"));
        }
    }
}