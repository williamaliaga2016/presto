using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA
{
    public class DevolucionVbComercialRepository : IDevolucionVbComercialRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public DevolucionVbComercialRepository(
            MultibancaDBContext _multibancaDBContext
        )
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<devolucion_vb_comercial_entity?> GetByExpediente(long id_expediente)
        {
            var data = new { p_id_expediente = id_expediente };
            DbConnection connection = MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open) await connection.OpenAsync();

                command.CommandText = "SELECT * FROM public.usp_select_devolucion_vb_comercial(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToDomain<devolucion_vb_comercial_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Devolución VB Comercial del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) await connection.CloseAsync();
            }
        }

        public async Task<devolucion_vb_comercial_entity> Guardar(devolucion_vb_comercial_entity actividad, int userId)
        {
            var actual = await MultibancaDBContext.devolucion_vb_comercial_entity
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

                await MultibancaDBContext.devolucion_vb_comercial_entity.AddAsync(actividad);

                return actividad;
            }

            actual.cliente_desiste = actividad.cliente_desiste;
            actual.motivo_cierre = actividad.motivo_cierre;
            actual.observaciones = actividad.observaciones;
            actual.modified_by = userId;
            actual.modified_date = DateTime.Now;

            return actual;
        }
    }
}
