using Microsoft.AspNetCore.Mvc;
using TestMagnetron.Models;
using TestMagnetron.Helpers;
using System.Net;
using System.Runtime.ConstrainedExecution;

namespace TestMagnetron.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        #region "Create"
        /// <summary>
        /// Metodo para agregar una Factura.
        /// </summary>
        /// <param name="factura">Factura a agregar con su detalle</param>
        /// <returns>Factura agregada</returns>
        /// <response code="409">Si la Factura ya existe</response>
        [ProducesResponseType(typeof(FactEncabezado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.Conflict)]
        [HttpPost("AgregarFactura")]
        public IActionResult AgregarFactura([FromBody] FactEncabezadoInputVM factura)
        {
            try
            {
                return Ok(FacturaHelper.AgregarFactura(factura));
            }
            catch (Exception ex)
            {
                string message = "Error al agregar la Factura: " + ex.Message;
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
        #region "Read"
        /// <summary>
        /// Metodo usado para obtener las facturas guardadas (encabezado y detalles).
        /// </summary>
        /// <returns>Una lista de Facturas.</returns>
        [HttpGet("Facturas")]
        [ProducesResponseType(typeof(IEnumerable<FactEncabezado>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerFacturas()
        {
            try
            {
                return Ok(FacturaHelper.ObtenerFacturas());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo usado para obtener una factura por Numero de factura
        /// </summary>
        /// <param name="numero">Numero de Factura a buscar</param>
        /// <returns>Factura encontrada.</returns>
        /// <response code="404">Si el producto no existe</response>
        [HttpGet("FacturaPorNumero")]
        [ProducesResponseType(typeof(FactEncabezado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ObtenerFacturaPorNumero(string numero)
        {
            try
            {
                if (string.IsNullOrEmpty(numero))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Número de factura vacio" });
                }
                else
                {
                    return Ok(FacturaHelper.ObtenerFacturaPorNumero(numero));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel { error = ex.Message });
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }
        #endregion
        #region "Update"
        /// <summary>
        /// Metodo para actualizar el encabezado de una Factura de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD de la factura a actualizar</param>
        /// <param name="nuevoEncab">Nuevo objeto con los valores a actualizar de la factura</param>
        /// <returns>Encabezado de la factura actualizado.</returns>
        /// <response code="404">Si la factura no existe</response>
        [HttpPut("ActualizarFactura")]
        [ProducesResponseType(typeof(FactEncabezado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ActualizarFactura(int id, [FromBody] FactEncabezadoUpdateVM nuevoEncab)
        {
            try
            {
                if (id == 0 )
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id" });
                }
                else
                {
                    return Ok(FacturaHelper.ActualizarFactura(id, nuevoEncab));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel { error = ex.Message });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }
        }

        /// <summary>
        /// Metodo para actualizar el detalle de una Factura de la BD
        /// </summary>
        /// <param name="id">Id del registro del detalle de la factura a actualizar en la BD</param>
        /// <param name="nuevoDetalle">Nuevo objeto con los valores a actualizar del detalle de la factura</param>
        /// <returns>Detalle de la factura actualizado.</returns>
        /// <response code="404">Si el detalle de la factura no existe</response>
        [HttpPut("ActualizarDetalleFactura")]
        [ProducesResponseType(typeof(FactDetalle), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ActualizarDetalleFactura(int id, [FromBody] FactDetalleUpdateVM nuevoDetalle)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id" });
                }
                else
                {
                    return Ok(FacturaHelper.ActualizarDetalleFactura(id, nuevoDetalle));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel { error = ex.Message });
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
        /// Metodo para eliminar una factura, se debe enviar el Id de base de datos o el número de factura
        /// </summary>
        /// <param name="id">Id del registro en la BD de la factura a eliminar (opcional)</param>
        /// <param name="numero">Numero de la factura a eliminar (opcional)</param>
        /// <returns>Si se elimina o no la persona</returns>
        /// <response code="404">Si la factura no existe</response>
        [HttpDelete("EliminarFactura")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult EliminarFactura(int id = 0, string? documento = null)
        {
            try
            {
                if (string.IsNullOrEmpty(documento) && id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id o un Número de factura" });
                }
                else
                {
                    return Ok(FacturaHelper.EliminarFactura(id, documento));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new ErrorModel { error = ex.Message});
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }
        }

        /// <summary>
        /// Metodo para eliminar un detalle de una factura de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD del detalle de la factura a eliminar (opcional)</param>
        /// <returns>Si se elimina o no la persona</returns>
        /// <response code="404">Si la factura no existe</response>
        [HttpDelete("EliminarDetalleFactura")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult EliminarDetalleFactura(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id" });
                }
                else
                {
                    return Ok(FacturaHelper.EliminarDetalleFactura(id));
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new ErrorModel { error = ex.Message });
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
