using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Common;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleApplication RoleApplicationProvider;

        public RoleController(IRoleApplication _roleApplication)
        {
            RoleApplicationProvider = _roleApplication;
        }

        [HttpGet, Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {

                var list_roles = RoleApplicationProvider.All();
                return Ok(new
                {
                    status = true,
                    detail = list_roles,
                    message = "Rol creado con éxito."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Insert([FromBody] roles role)
        {
            try
            {
                //role.created_by = GetUserId();
                role.created_date = DateTime.Now;

                var res = RoleApplicationProvider.Create(role,1);
                //await RoleApplicationProvider.InsUdpRoleMenu(res.role_id);
                return Ok(new
                {
                    status = true,
                    detail = res,
                    message = "Rol creado con éxito."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update([FromBody] roles role)
        {
            try
            {
                //role.modified_by = GetUserId();
                role.modified_date = DateTime.Now;
                var res = RoleApplicationProvider.Update(role, 1);
                //await RoleApplicationProvider.InsUdpRoleMenu(role.role_id);
                return Ok(new
                {
                    status = true,
                    detail = res,
                    message = "Rol actualizado con éxito."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [HttpDelete, Route("Delete/{role_id}")]
        public async Task<IActionResult> Delete(int role_id)
        {
            try
            {
                RoleApplicationProvider.LogicalDeleteById(role_id, 1);
                return Ok(new
                {
                    status = true,
                    detail = true,
                    message = "Rol eliminado con éxito."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        /******* Controls ********/

        [HttpGet, Route("GetControlRole")]
        public async Task<IActionResult> GetControlRole()
        {
            try
            {
                List<ControlBaseDTO> list = await RoleApplicationProvider.GetControlRole();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region GetUserId
        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
        #endregion
    }
}
