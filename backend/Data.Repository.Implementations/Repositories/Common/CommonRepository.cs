using Data.Extensions.Repository;
using Data.Repository.Interfaces.Repositories.Common;
using Framework.WorkFlow.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Common;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public CommonRepository(MultibancaDBContext _multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<FolioDTO> CapturarDatosFolio(long id_expediente, string id_actividad)
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

                command.CommandText = "SELECT * FROM sp_get_info_folio(@p_id_expediente, @p_id_actividad);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<FolioDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al capturar datos del folio para el expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<AssignActivityDTO> AsignarActividad(long id_expediente, string id_performer)
        {
            var data = new
            {
                p_id_expediente = id_expediente,
                p_id_performer = id_performer
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_actividad_asignar(@p_id_expediente, @p_id_performer);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<AssignActivityDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener actividad asignada para expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoByType(string tipo, string? codigoPadre = null)
        {
            object data = new
            {
                p_type = tipo,
                p_codigo_padre = codigoPadre
            };

            List<ControlBaseDTO> listControlBaseDTO = new();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT *
                FROM usp_get_catalogo_by_tipo(
                    @p_type,
                    @p_codigo_padre
                );";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlBaseDTO = reader.MapToListDomain<ControlBaseDTO>();
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }
            }

            return listControlBaseDTO;
        }

        /// <inheritdoc />
        public async Task<List<ControlBaseDTO>> GetCatalogoByTypeWithParentCode(string tipo)
        {
            object data = new
            {
                p_type = tipo
            };

            List<ControlBaseDTO> listControlBaseDTO = new List<ControlBaseDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            c.id::integer AS id,
                            c.id::bigint AS idBig,
                            c.valor AS code,
                            c.descripcion AS description,
                            p.valor AS parent_code
                        FROM public.catalogo c
                        LEFT JOIN public.catalogo p ON p.id = c.id_padre
                        WHERE c.tipo = @p_type
                          AND c.is_active = true
                        ORDER BY c.orden NULLS LAST, c.id;
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlBaseDTO = reader.MapToListDomain<ControlBaseDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener catalogo con padre por tipo {tipo}: {ex.Message}", ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listControlBaseDTO;
            }
        }

        public async Task<bool> ExisteActividadFolio(long idExpediente, string idActividad)
        {
            var data = new
            {
                p_id_expediente = idExpediente,
                p_id_actividad = idActividad
            };

            DbConnection connection = MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT sp_existe_actividad_folio(@p_id_expediente, @p_id_actividad);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                var result = await command.ExecuteScalarAsync();

                return result != null && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al consultar la actividad '{idActividad}' del expediente {idExpediente}.",
                    ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
