using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA
{
    public class ValidarIntegracionRepository : IValidarIntegracionRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ValidarIntegracionRepository(
            MultibancaDBContext _multibancaDBContext
        )
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<validar_integracion_documentos_entity?> GetByExpediente(
            long id_expediente
        )
        {
            // Creamos el objeto anónimo para los parámetros de la función de PostgreSQL
            var data = new
            {
                p_id_expediente = id_expediente
            };

            DbConnection connection = MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                command.CommandText = @"
                    SELECT * FROM public.usp_select_validar_integracion_documentos(
                        @p_id_expediente
                    );
                ";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data); // Usa el método de extensión propio de tu proyecto para parsear los parámetros

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToDomain<validar_integracion_documentos_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener la Validación de Integración de Documentos del expediente {id_expediente}.",
                    ex
                );
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<validar_integracion_documentos_entity> Guardar(validar_integracion_documentos_entity actividad, int userId)
        {
            var actual = await MultibancaDBContext.validar_integracion_documentos_entity
                .FirstOrDefaultAsync(x =>
                    x.id_expediente == actividad.id_expediente &&
                    x.is_active &&
                    x.row_status);

            if (actual == null)
            {
                actividad.is_active = true;
                actividad.row_status = true;
                actividad.created_by = userId;
                actividad.created_date = DateTime.Now;

                await MultibancaDBContext.validar_integracion_documentos_entity.AddAsync(actividad);

                return actividad;
            }

            actual.documentos_correctos = actividad.documentos_correctos;
            actual.credito_condicionado = actividad.credito_condicionado;
            actual.motivo_devolucion = actividad.motivo_devolucion;
            actual.observaciones = actividad.observaciones;
            actual.modified_by = userId;
            actual.modified_date = DateTime.Now;

            return actual;
        }

        #region INTERVINIENTE

        public async Task<List<interviniente_bbva_entity>> GetIntervinientes(long idExpediente)
        {
            return await MultibancaDBContext.interviniente_bbva_entity
                .Where(x =>
                    x.id_expediente == idExpediente &&
                    x.is_active &&
                    x.row_status)
                .OrderByDescending(x => x.created_date)
                .ToListAsync();
        }

        public async Task<interviniente_bbva_entity?> GetInterviniente(int id)
        {
            return await MultibancaDBContext.interviniente_bbva_entity
                .FirstOrDefaultAsync(x =>
                    x.id == id &&
                    x.is_active &&
                    x.row_status);
        }

        public async Task<interviniente_bbva_entity> GuardarInterviniente(interviniente_bbva_entity interviniente, int userId)
        {
            var actual = await MultibancaDBContext.interviniente_bbva_entity
                .FirstOrDefaultAsync(x =>
                    x.id == interviniente.id &&
                    x.is_active &&
                    x.row_status);

            if (actual == null)
            {
                interviniente.is_active = true;
                interviniente.row_status = true;
                interviniente.created_by = userId;
                interviniente.created_date = DateTime.Now;

                await MultibancaDBContext.interviniente_bbva_entity.AddAsync(interviniente);

                return interviniente;
            }

            actual.nombre_completo = interviniente.nombre_completo;
            actual.tipo_identificacion = interviniente.tipo_identificacion;
            actual.numero_identificacion = interviniente.numero_identificacion;
            actual.telefono = interviniente.telefono;
            actual.correo_electronico = interviniente.correo_electronico;

            actual.modified_by = userId;
            actual.modified_date = DateTime.Now;

            return actual;
        }

        public async Task<bool> CambiarEstadoInterviniente(int id, bool activo, int userId)
        {
            var actual = await MultibancaDBContext.interviniente_bbva_entity
                .FirstOrDefaultAsync(x => x.id == id);

            if (actual == null)
                return false;

            actual.is_active = activo;
            actual.modified_by = userId;
            actual.modified_date = DateTime.Now;

            return true;
        }


        #endregion
    }
}
