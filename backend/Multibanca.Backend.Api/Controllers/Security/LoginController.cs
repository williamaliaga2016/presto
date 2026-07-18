        using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Multibanca.Backend.Api.Controllers.Security
{
    [Route("api/security/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ILoginApplication LoginApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;

        public LoginController(IConfiguration _configuration, ILoginApplication _loginApplication, IUserApplication _userApplication)
        {
            Configuration = _configuration;
            LoginApplicationProvider = _loginApplication;
            UserApplicationProvider = _userApplication;
        }

        [HttpPost, Route("auth")]
        //[ServiceFilter(typeof(ReCaptchaValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (loginDTO == null) return BadRequest("Invalid client request");

                AuthUserDTO authUserDTO = await LoginApplicationProvider.Login(loginDTO);

                if (authUserDTO == null)
                {
                    string res = await GetUserByUserName(loginDTO.user_name);

                    if (res.Contains("BadRequest"))
                    {
                        return BadRequest("Error 401: Bad Request, intente nuevamente.");
                    }

                    return Ok(new
                    {
                        status = true,
                        detail = "FALLO",
                        message = "Error 401: Bad Request, intente nuevamente."
                    });
                }
                else
                {
                    ////    if (user.isFirstAccess)
                    ////    {
                    ////        return Ok(new
                    ////        {
                    ////            status = true,
                    ////            detail = string.Empty,
                    ////            message = "Acceso por primera vez, favor de cambiar su contraseña."
                    ////        });
                    ////    }

                    ClaimsIdentity identity = await GetClaimsIdentity(authUserDTO);
                    authUserDTO.token_multibanca = GetJwtToken(identity);
                    return Ok(new
                    {
                        status = true,
                        detail = authUserDTO,
                        message = authUserDTO.message
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Ok(new
                {
                    status = false,
                    detail = ex.Message,
                    message = "Usuario o Rol NO EXISTE!"
                });
            }
        }

        private async Task<string> GetUserByUserName(string user_name)
        {
            users user = await LoginApplicationProvider.GetUserByUserName(user_name);
            users userResult = new users();
            string resMsg = string.Empty;

            if (user != null)
            {
                if (user.is_active)
                {
                    switch (user.remaining_attempts)
                    {
                        case 1:
                            user.remaining_attempts--;
                            user.is_active = false;
                            user.is_first_access = true;
                            UserApplicationProvider.Update(user, user.user_id);
                            resMsg = "Último intento fallido, usuario INACTIVO!";
                            break;
                        case 2:
                            user.remaining_attempts--;
                            UserApplicationProvider.Update(user, user.user_id);
                            resMsg = "Verifique sus credenciales, le queda 1 intento, se INACTIVARA el usuario.";
                            break;
                        case 3:
                            user.remaining_attempts--;
                            UserApplicationProvider.Update(user, user.user_id);
                            resMsg = "Verifique sus credenciales, le quedan 2 intentos, se INACTIVARA el usuario.";
                            break;
                        default:
                            resMsg = "Usuario INACTIVO!";
                            break;
                    }
                }
                else
                {
                    resMsg = "Usuario INACTIVO!";
                }
            }
            else
            {
                resMsg = "BadRequest";
            }

            return resMsg;
        }

        private string GetJwtToken(ClaimsIdentity identity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Issuer"],
                identity.Claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(AuthUserDTO authUserDTO)
        {
            Claim[] claims = new[]
            {
                new Claim("user_name", authUserDTO.user_name),
                new Claim("email", authUserDTO.email),
                new Claim("user_id", authUserDTO.user_id.ToString()),
                new Claim("name_complete", authUserDTO.name_complete),
                new Claim("role_id", authUserDTO.role_id != null ? authUserDTO.role_id.ToString(): "0"),
                new Claim("performer", String.IsNullOrEmpty(authUserDTO.code)?"":authUserDTO.code),
                new Claim(ClaimTypes.Role, authUserDTO.role_name),
                new Claim(ClaimTypes.NameIdentifier, authUserDTO.user_id.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "token_multibanca");

            return claimsIdentity;
        }
    }
}
