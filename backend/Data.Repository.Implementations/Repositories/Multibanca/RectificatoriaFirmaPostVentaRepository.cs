using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class RectificatoriaFirmaPostVentaRepository
        : MultibancaGenericRepository<rectificatoria_firma_post_venta_entity>,
          IRectificatoriaFirmaPostVentaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RectificatoriaFirmaPostVentaRepository(
            MultibancaDBContext _multibancaDBContext
        ) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<rectificatoria_firma_post_venta_entity?> GetByExpediente(
            long id_expediente
        )
        {
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
                    SELECT *
                    FROM public.usp_select_rectificatoria_firma_post_venta(
                        @p_id_expediente
                    );
                ";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToDomain<rectificatoria_firma_post_venta_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Rectificatoria Firma post venta del expediente {id_expediente}.",
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

        public async Task<List<rectificatoria_firma_post_venta_detalle_entity>> GetRectificatoriaDetByExpediente(
            long id_expediente
        )
        {
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
                    SELECT *
                    FROM public.usp_select_rectificatoria_firma_post_venta_detalle(
                        @p_id_expediente
                    );
                ";

                command.CommandType = CommandType.Text;
                command.Parameters.ToArray(data);

                await using var reader = await command.ExecuteReaderAsync();

                return reader.MapToListDomain<rectificatoria_firma_post_venta_detalle_entity>();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al obtener Rectificatoria Firma post venta Detalle del expediente {id_expediente}.",
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
    }
}
