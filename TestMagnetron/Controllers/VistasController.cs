using Microsoft.AspNetCore.Mvc;
using TestMagnetron.Models;
using TestMagnetron.Helpers;
using System.Net;
using TestMagnetron.Models.Views;

namespace TestMagnetron.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VistasController : ControllerBase
    {
        /// <summary>
        /// Metodo para obtener la lista de margen de ganancia de cada producto
        /// </summary>
        /// <returns>Una lista basado en la vista.</returns>
        [HttpGet("VistaMargenGananciaPorProducto")]
        [ProducesResponseType(typeof(IEnumerable<VistaMargenGananciaPorProducto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerVistaMargenGananciaPorProducto()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    return Ok(db.VistaMargenGananciaPorProductos.ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo para obtener la lista de lapersona que haya comprado el producto mas caro
        /// </summary>
        /// <returns>Una lista basado en la vista.</returns>
        [HttpGet("VistaPersonaProductoMasCaro")]
        [ProducesResponseType(typeof(IEnumerable<VistaPersonaProductoMasCaro>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerVistaPersonaProductoMasCaro()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    return Ok(db.VistaPersonaProductoMasCaros.ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo para obtener la info de productos por cantidad facturada
        /// </summary>
        /// <returns>Una lista basado en la vista.</returns>
        [HttpGet("VistaProductosPorCantidadFactura")]
        [ProducesResponseType(typeof(IEnumerable<VistaProductosPorCantidadFacturadum>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerVistaProductosPorCantidadFactura()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    return Ok(db.VistaProductosPorCantidadFacturada.ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo para obtener la info del total facturado por persona
        /// </summary>
        /// <returns>Una lista basado en la vista.</returns>
        [HttpGet("VistaTotalFacturado")]
        [ProducesResponseType(typeof(IEnumerable<VistaTotalFacturado>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerVistaTotalFacturado()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    return Ok(db.VistaTotalFacturados.ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo para obtener la info de la vista utilidad por producto
        /// </summary>
        /// <returns>Una lista basado en la vista.</returns>
        [HttpGet("VistaUtilidadPorProducto")]
        [ProducesResponseType(typeof(IEnumerable<VistaUtilidadPorProducto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerVistaUtilidadPorProducto()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    return Ok(db.VistaUtilidadPorProductos.ToList());
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

    }
}
