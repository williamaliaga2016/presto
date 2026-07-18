using Data.Extensions.Repository;
using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Security
{
    public class MenuRepository : MultibancaGenericRepository<menus_entity>, IMenuRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;
        public MenuRepository(MultibancaDBContext _skayrosDBContext) : base(_skayrosDBContext)
        {
            MultibancaDBContext = _skayrosDBContext;
        }

        public async Task<List<MenuItemDTO>> GetMenuJerarquico()
        {
            List<MenuItemDTO> list = new List<MenuItemDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    MultibancaDBContext.Database.OpenConnection();
                    command.CommandText = "spObtenerMenuJerarquico";
                    command.CommandType = CommandType.StoredProcedure;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = (reader.MapToListDomain<MenuItemDTO>()) ?? list;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    MultibancaDBContext.Database.CloseConnection();
                }

                return BuildTree(list);
            }
        }

        public async Task<List<MenuItemDTO>> GetMenuByUserId(int userId)
        {
            List<MenuItemDTO> list = new List<MenuItemDTO>();
            object data = new
            {
                p_userId = userId
            };

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    MultibancaDBContext.Database.OpenConnection();
                    command.CommandText = "spObtenerMenuPorUsuario";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.ToArray(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = (reader.MapToListDomain<MenuItemDTO>()) ?? list;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    MultibancaDBContext.Database.CloseConnection();
                }

                return BuildTree(list); ;
            }
        }

        public async Task<List<MenuItemDTO>> GetMenuByRoleId(int role_id)
        {
            List<MenuItemDTO> list = new List<MenuItemDTO>();
            object data = new
            {
                p_roleId = role_id
            };

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    MultibancaDBContext.Database.OpenConnection();
                    command.CommandText = "spObtenerMenuPorRole";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.ToArray(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = (reader.MapToListDomain<MenuItemDTO>()) ?? list;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    MultibancaDBContext.Database.CloseConnection();
                }

                return BuildTree(list); ;
            }
        }

        private List<MenuItemDTO> BuildTree(List<MenuItemDTO> planos)
        {
            var map = planos.ToDictionary(m => m.menuId, m => new MenuItemDTO
            {
                menuId = m.menuId,
                name = m.name,
                menuUrl = m.menuUrl,
                is_active = m.is_active,
                disabled = m.disabled,
                menuPadreId = m.menuPadreId
            });

            var raiz = new List<MenuItemDTO>();

            foreach (var item in map.Values)
            {
                if (item.menuPadreId.HasValue && map.ContainsKey(item.menuPadreId.Value))
                    map[item.menuPadreId.Value].children.Add(item);
                else
                    raiz.Add(item);
            }

            return raiz;
        }
    }
}
