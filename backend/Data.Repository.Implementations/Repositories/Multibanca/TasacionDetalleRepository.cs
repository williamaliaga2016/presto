using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class TasacionDetalleRepository : MultibancaGenericRepository<tasacion_detalle_entity>, ITasacionDetalleRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public TasacionDetalleRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<tasacion_detalle_entity>> GetByExpediente(long id_expediente)
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

                command.CommandText = "SELECT * FROM usp_select_tasacion_detalle_by_expediente(@p_id_expediente);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<tasacion_detalle_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Tasación Detalle del expediente {id_expediente}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<List<tasacion_detalle_entity>> GetByTasacion(int id_tasacion)
        {
            var data = new
            {
                p_id_tasacion = id_tasacion
            };

            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = "SELECT * FROM usp_select_tasacion_detalle_by_tasacion(@p_id_tasacion);";
                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();
                return reader.MapToListDomain<tasacion_detalle_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Tasación Detalle por id_tasacion {id_tasacion}.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
