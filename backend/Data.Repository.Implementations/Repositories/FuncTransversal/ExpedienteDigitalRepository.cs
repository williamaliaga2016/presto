using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Common;
using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.FuncTransversal
{
    public class ExpedienteDigitalRepository : MultibancaGenericRepository<expediente_digital_entity>, IExpedienteDigitalRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ExpedienteDigitalRepository(MultibancaDBContext multibancaDBContext) : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<bool> ExistsByIdAsync(long idExpediente)
        {
            return await MultibancaDBContext.ExpedienteDigitalEntities
                .AnyAsync(x => x.id_expediente == idExpediente && x.is_active);
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoCategoriaExpedienteDigital()
        {
            List<ControlBaseDTO> listControlBaseDTO = new List<ControlBaseDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM usp_select_catalogo_categorias_expediente_digital();
                ";

                    command.CommandType = CommandType.Text;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlBaseDTO = reader.MapToListDomain<ControlBaseDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listControlBaseDTO;
            }
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoDocumentos(int idExpedienteDigital, long idExpediente)
        {
            object data = new
            {
                p_id_expediente_digital = idExpedienteDigital,
                p_id_expediente = idExpediente
            };

            List<ControlBaseDTO> listControlDocumento = new List<ControlBaseDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM usp_select_catalogo_documentos(
                        @p_id_expediente_digital,
                        @p_id_expediente
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlDocumento = reader.MapToListDomain<ControlBaseDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listControlDocumento;
            }
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoDocumentosCompleto()
        {
            List<ControlBaseDTO> listControlBaseDTO = new List<ControlBaseDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM usp_select_catalogo_documentos_completo();
                ";

                    command.CommandType = CommandType.Text;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlBaseDTO = reader.MapToListDomain<ControlBaseDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listControlBaseDTO;
            }
        }

        public async Task<List<expediente_digital_entity>> GetFilesExpedienteDigital(long idExpediente)
        {
            object data = new
            {
                p_id_expediente = idExpediente
            };

            List<expediente_digital_entity> expediente_digital_entity = new List<expediente_digital_entity>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM select_archivos_expediente_digital(
                        @p_id_expediente
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    expediente_digital_entity = reader.MapToListDomain<expediente_digital_entity>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return expediente_digital_entity;
            }
        }

        public async Task<string> FileNameVersion(long idExpediente, long idDocumento)
        {
            object data = new
            {
                p_id_expediente = idExpediente,
                p_id_documento = Convert.ToInt32(idDocumento)
            };

            string result = string.Empty;

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM usp_select_file_name_version(
                        @p_id_expediente,
                        @p_id_documento
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = reader["nombre_documento"]?.ToString() ?? string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return result;
            }
        }


        public async Task<string> GetDocumentoNombreCorto(long idDocumento)
        {
            object data = new
            {
                p_id_documento = Convert.ToInt32(idDocumento)
            };

            string result = string.Empty;

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM public.usp_select_documento_nombre_corto(
                        @p_id_documento
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = reader["nombre_corto"]?.ToString() ?? string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return result;
            }
        }

        public async Task<UserResponsibleDTO> GetUserResponsibleByIdExpediente(long idExpediente)
        {
            object data = new
            {
                p_id_expediente = idExpediente
            };

            UserResponsibleDTO userResponsibleDTO = new UserResponsibleDTO();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM sp_get_user_responsible_by_id_expediente(
                        @p_id_expediente
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    userResponsibleDTO = reader.MapToDomain<UserResponsibleDTO>();
                }
                catch (Exception ex)
                {
                    userResponsibleDTO = null;
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return userResponsibleDTO;
            }
        }

        public async Task<string> GetTemplateFileName(int idCatExpedienteDigital, int idCatExpedienteDigitalDocumento)
        {
            object data = new
            {
                p_id_cat_expediente_digital = idCatExpedienteDigital,
                p_id_cat_expediente_digital_documento = idCatExpedienteDigitalDocumento
            };

            string templates = string.Empty;

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                    SELECT *
                    FROM usp_get_template_file_name(
                        @p_id_cat_expediente_digital,
                        @p_id_cat_expediente_digital_documento
                    );
                ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        templates = reader["template_file_name"]?.ToString() ?? string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    templates = null;
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return templates;
            }
        }
    }
}
