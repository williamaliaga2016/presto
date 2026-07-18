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
    public class BandejaActividadesRepository : IBandejaActividadesRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public BandejaActividadesRepository(MultibancaDBContext _multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<ActividadDTO>> GetInfoActivityByUser(long idUsuario, string workFlowProcessID)
        {
            var data = new
            {
                p_id_usuario = Convert.ToInt32(idUsuario),
                p_work_flow_process_id = workFlowProcessID
            };

            List<ActividadDTO> listActividades = new();
            DbConnection connection = this.MultibancaDBContext.Database.GetDbConnection();
            await using DbCommand command = connection.CreateCommand();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                command.CommandText = @"
            SELECT * 
            FROM sp_get_act_dashboard_details(
                @p_id_usuario,
                @p_work_flow_process_id
            );";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using DbDataReader reader = await command.ExecuteReaderAsync();
                listActividades = reader.MapToListDomain<ActividadDTO>();

                return listActividades;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el dashboard de actividades.", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
