using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestMagnetron.Models;
using TestMagnetron.Helpers;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection.Metadata;

namespace TestMagnetron.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Persona> GetPersonas()
        {
            return PersonaHelper.ObtenerPersonas();
            
        }
        [HttpGet("PersonaPorDocumento")]
        public Persona GetPersonasByDocumento(string documento)
        {
            if (string.IsNullOrEmpty(documento))
            {
                _logger.LogError("Documento no valido");
                throw new BadRe
            }
            else
            {
                return
            }
            
        }

        /// <summary>
        /// Retorna un Codigo Http basado en un tipo de error
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="message">Mensaje de Error</param>
        /// <param name="methodName">Nombre del metodo</param>
        /// <param name=" receivedObject">objeto recibido</param>
        /// <returns>Error Model</returns>
        private ObjectResult ReturnError(Exception ex, string message, string methodName, object receivedObject)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case System.Data.DuplicateNameException:
                    httpStatusCode = HttpStatusCode.Conflict;
                    break;
                case UnauthorizedException:
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    break;
                case ForbiddenException:
                    httpStatusCode = HttpStatusCode.Forbidden;
                    break;
                case BadRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return StatusCode((int)httpStatusCode, new ErrorModel { error = message });
        }

    }
}
