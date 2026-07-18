using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Common;
using Multibanca.Domain.Models.Security;
using Newtonsoft.Json;

namespace Multibanca.Backend.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication UserApplicationProvider;

        public UserController(IUserApplication _userApplication)
        {
            UserApplicationProvider = _userApplication;
        }

        [HttpGet, Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            IActionResult response = Unauthorized();

            try
            {
                List<users> listUsers = UserApplicationProvider.All().ToList();

                string modelResult = JsonConvert.SerializeObject(listUsers, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                listUsers = JsonConvert.DeserializeObject<List<users>>(modelResult);

                if (listUsers != null)
                {
                    return Ok(new
                    {
                        status = true,
                        detail = listUsers,
                        message = string.Empty
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        detail = new List<users>(),
                        message = "No existen usuarios!"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] users user)
        {
            if (user == null) return BadRequest("Invalid client request");

            try
            {
                user.created_by = 1;
                user.created_date = DateTime.Now;
                user.modified_by = null;
                user.modified_date = null;
                user.password = EncryptHelper.GenerateHash(user.password);

                users userResponse = UserApplicationProvider.Create(user, user.created_by);

                if (userResponse != null)
                {
                    string modelResult = JsonConvert.SerializeObject(userResponse, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    userResponse = JsonConvert.DeserializeObject<users>(modelResult);

                    return Ok(new
                    {
                        status = true,
                        detail = userResponse,
                        message = "Usuario guardado!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        detail = new users(),
                        message = "No existen usuarios!"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update([FromBody] users user)
        {
            if (user == null) return BadRequest("Invalid client request");

            try
            {
                user.modified_by = 1;
                user.modified_date = DateTime.Now;
                string currentPass = UserApplicationProvider.FindId(user.user_id).password;
                if (!EncryptHelper.ValidateHash(currentPass, user.password))
                {
                    if (user.password != currentPass)
                    {
                        user.password = EncryptHelper.GenerateHash(user.password);
                    }
                }
                else
                {
                    user.password = currentPass;
                }

                users userResponse = UserApplicationProvider.Update(user, user.modified_by);

                if (userResponse != null)
                {
                    string modelResult = JsonConvert.SerializeObject(userResponse, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    userResponse = JsonConvert.DeserializeObject<users>(modelResult);                    

                    return Ok(new
                    {
                        status = true,
                        detail = userResponse,
                        message = "Usuario actualizado!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        detail = new users(),
                        message = "No existen usuario!"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("Delete/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                UserApplicationProvider.LogicalDeleteById(userId, 1);
                return Ok(new
                {
                    status = true,
                    detail = true,
                    message = "Usuario eliminado con éxito."
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
    }
}
