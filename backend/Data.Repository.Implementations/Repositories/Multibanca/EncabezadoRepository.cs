using Data.Extensions.Repository;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class EncabezadoRepository : IEncabezadoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public EncabezadoRepository(MultibancaDBContext _multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<EncabezadoDTO> InformacionEncabezado(long idExpediente, string activityID)
        {
            var data = new
            {
                p_id_expediente = idExpediente,
                p_activity_id = activityID
            };

            EncabezadoDTO encabezadoInfo = new();
            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using DbCommand command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = @"
            SELECT *
            FROM usp_get_encabezado_bbva(
                @p_id_expediente,
                @p_activity_id
            );";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using DbDataReader reader = await command.ExecuteReaderAsync();
                encabezadoInfo = reader.MapToDomain<EncabezadoDTO>();

                // Los campos BBVA (id_scoring, titulares, oficina, subproducto, inmueble...)
                // ya no se resuelven aquí: EncabezadoApplication los completa desde las
                // tablas de carga_operacion_banco por id_expediente, sin depender de activityID.

                return encabezadoInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la información de encabezado.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
