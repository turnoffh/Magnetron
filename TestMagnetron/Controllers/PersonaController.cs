using Microsoft.AspNetCore.Mvc;
using TestMagnetron.Models;
using TestMagnetron.Helpers;
using System.Net;

namespace TestMagnetron.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        #region "Create"
        /// <summary>
        /// Metodo para agregar una persona.
        /// </summary>
        /// <param name="per">Persona a agregar</param>
        /// <returns>Modelo de la persona recien agregada</returns>
        /// <response code="409">Si la persona ya existe</response>
        [ProducesResponseType(typeof(Persona), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.Conflict)]
        [HttpPost("AgregarUsuario")]
        public IActionResult AgregarPersona([FromBody] PersonaInputVM per)
        {
            Persona personaRetorno;

            try
            {
                personaRetorno = PersonaHelper.AgregarPersona(per);
            }
            catch (Exception ex)
            {
                string message = "Error al agregar al usuario: " + ex.Message;
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new ErrorModel { error = message });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }

            return Ok(personaRetorno);
        }

        #endregion
        #region "Read"
        /// <summary>
        /// Metodo usado para obtener las personas guardadas.
        /// </summary>
        /// <returns>Una lista de personas.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Persona>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerPersonas()
        {
            try
            {
                return Ok(PersonaHelper.ObtenerPersonas());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo usado para obtener una persona por numero de documento.
        /// </summary>
        /// <param name="documento">Documento a buscar</param>
        /// <returns>Persona buscada.</returns>
        [HttpGet("PersonaPorDocumento")]
        [ProducesResponseType(typeof(Persona), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerPersonasPorDocumento(string documento)
        {
            try
            {
                if (string.IsNullOrEmpty(documento))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Número de documento vacio" });
                }
                else
                {
                    return Ok(PersonaHelper.ObtenerPersonaPorDocumento(documento));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo usado para obtener una persona por nombre.
        /// </summary>
        /// <param name="nombre">Nombre a buscar</param>
        /// <returns>Una lista de personas.</returns>
        [HttpGet("PersonaPorNombre")]
        [ProducesResponseType(typeof(List<Persona>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerPersonasPorNombreoApellido(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Valor a buscar vacio" });
                }
                else
                {
                    return Ok(PersonaHelper.ObtenerPersonaPorNombreOApellido(nombre));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }
        #endregion
        #region "Update"
        /// <summary>
        /// Metodo para actualizar una persona, se debe enviar el Id de base de datos o el número de documento
        /// </summary>
        /// <param name="id">Id del registro en la BD de la persona a actualizar (opcional)</param>
        /// <param name="documento">Documento de la persona a actualizar (opcional)</param>
        /// <param name="persona">Objeto con los valores nuevos a actualizar</param>
        /// <returns>Persona actualizada.</returns>
        /// <response code="409">Si la persona no existe</response>
        [HttpPut("ActualizarPersona")]
        [ProducesResponseType(typeof(Persona), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.Conflict)]
        public IActionResult ActualizarPersona([FromBody] PersonaUpdateVM persona, int id = 0, string? documento = null)
        {
            try
            {
                if (string.IsNullOrEmpty(documento) && id == 0 )
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id o un Número de Documento" });
                }
                else
                {
                    return Ok(PersonaHelper.ActualizarPersona(persona, id, documento));
                }
            }
            catch (Exception ex)
            {
                string message = "Error al actualizar el usuario: " + ex.Message;
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new ErrorModel { error = message });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }
        }
        #endregion
        #region "Delete"
        /// <summary>
        /// Metodo para eliminar una persona, se debe enviar el Id de base de datos o el número de documento
        /// </summary>
        /// <param name="id">Id del registro en la BD de la persona a actualizar (opcional)</param>
        /// <param name="documento">Documento de la persona a actualizar (opcional)</param>
        /// <returns>Si se elimina o no la persona</returns>
        /// <response code="409">Si la persona no existe</response>
        [HttpDelete("EliminarPersona")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.Conflict)]
        public IActionResult EliminarPersona(int id = 0, string? documento = null)
        {
            try
            {
                if (string.IsNullOrEmpty(documento) && id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id o un Número de Documento" });
                }
                else
                {
                    return Ok(PersonaHelper.EliminarPersona(id, documento));
                }
            }
            catch (Exception ex)
            {
                string message = "Error al eliminar el usuario: " + ex.Message;
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new ErrorModel { error = message });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }
        }
        #endregion
    }
}
