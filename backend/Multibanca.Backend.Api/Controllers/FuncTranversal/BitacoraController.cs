using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Domain.Models.FuncTransversal;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.FuncTranversal
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly IBitacoraApplication BitacoraApplicationProvider;

        public BitacoraController(IBitacoraApplication _bitacoraApplicationProvider)
        {
            BitacoraApplicationProvider = _bitacoraApplicationProvider;
        }

        [HttpGet, Route("GetByIdExpediente/{id_expediente}")]
        public async Task<IActionResult> GetBitacora(long id_expediente)
        {
            IActionResult response = Unauthorized();
            try
            {

                List<bitacora> listResult = BitacoraApplicationProvider.All().Where(q => q.id_expediente == id_expediente).ToList(); //id_expediente para pruebas
                if (listResult == null) return BadRequest("Invalid client request");
                foreach (bitacora bitacora in listResult)
                {
                    bitacora.usuario = "John Doe";
                    bitacora.rol = "Administrador";
                }                

                response = Ok(new
                {
                    status = true,
                    detail = listResult,
                    message = "Lista de bitácoras!!"
                });
            }
            catch (Exception ex)
            {
                response = StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }


            return response;

        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] bitacora bitacora)
        {
            IActionResult response = Unauthorized();

            try
            {
                bitacora.id_usuario = 1;
                bitacora.fecha_alta = DateTime.Now;
                bitacora result = BitacoraApplicationProvider.Create(bitacora, GetUserId());

                if (result != null)
                {
                    List<bitacora> listBitacoras = BitacoraApplicationProvider.All().Where(q => q.id_expediente == bitacora.id_expediente).ToList();
                    foreach (bitacora item in listBitacoras)
                    {
                        item.usuario = "John Doe";
                        item.rol = "Administrador";
                    }

                    response = Ok(new
                    {
                        status = true,
                        detail = listBitacoras,
                        message = "Bitácora guardada correctamente"
                    });
                }
            }
            catch (Exception ex)
            {
                response = StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }

            return response;
        }

        #region GetUserId
        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
        #endregion
    }
}
