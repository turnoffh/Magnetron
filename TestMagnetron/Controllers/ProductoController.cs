using Microsoft.AspNetCore.Mvc;
using TestMagnetron.Models;
using TestMagnetron.Helpers;
using System.Net;

namespace TestMagnetron.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        #region "Create"
        /// <summary>
        /// Metodo para agregar un producto.
        /// </summary>
        /// <param name="prod">Producto a agregar</param>
        /// <returns>Modelo del producto recien agregada</returns>
        /// <response code="409">Si el producto ya existe</response>
        [ProducesResponseType(typeof(Producto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.Conflict)]
        [HttpPost("AgregarProducto")]
        public IActionResult AgregarProducto([FromBody] ProductoInputVM prod)
        {
            Producto productoRetorno;

            try
            {
                productoRetorno = ProductoHelper.AgregarProducto(prod);
            }
            catch (Exception ex)
            {
                string message = "Error al agregar el producto: " + ex.Message;
                if (ex is InvalidOperationException)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new ErrorModel { error = message });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
                }
            }

            return Ok(productoRetorno);
        }

        #endregion
        #region "Read"
        /// <summary>
        /// Metodo usado para obtener los productos
        /// </summary>
        /// <returns>Una lista de productos.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Producto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ObtenerProductos()
        {
            try
            {
                return Ok(ProductoHelper.ObtenerProductos());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorModel { error = ex.Message });
            }
        }

        /// <summary>
        /// Metodo usado para obtener un producto por Id.
        /// </summary>
        /// <param name="Id">Id del registro del producto en la BD</param>
        /// <returns>Producto buscado.</returns>
        /// /// <response code="404">Si el producto no existe</response>
        [HttpGet("ProductoPorId")]
        [ProducesResponseType(typeof(Producto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ObtenerProductoPorId(int prodId)
        {
            try
            {
                if (prodId == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Id no valido " });
                }
                else
                {
                    return Ok(ProductoHelper.ObtenerProductoPorId(prodId));
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
        /// Metodo usado para obtener un producto por nombre.
        /// </summary>
        /// <param name="nombre">Nombre a buscar</param>
        /// <returns>Una lista de productos que contienen el nombre.</returns>
        /// <response code="404">Si el producto no existe</response>
        [HttpGet("ProductoPorNombre")]
        [ProducesResponseType(typeof(List<Producto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ObtenerProductosPorNombre(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Valor a buscar vacio" });
                }
                else
                {
                    return Ok(ProductoHelper.ObtenerProductoPorNombre(nombre));
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
        #region "Update"
        /// <summary>
        /// Metodo para actualizar un producto
        /// </summary>
        /// <param name="id">Id del registro en la BD del producto a actualizar</param>
        /// <param name="producto">Objeto con los valores nuevos a actualizar</param>
        /// <returns>Producto actualizado.</returns>
        /// <response code="404">Si el producto no existe</response>
        [HttpPut("ActualizarProducto")]
        [ProducesResponseType(typeof(Producto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult ActualizarProducto(int id, [FromBody] ProductoUpdateVM producto)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id" });
                }
                if (producto is null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un producto a actualizar" });
                }
                
                return Ok(ProductoHelper.ActualizarProducto(id, producto));
                
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
        /// Metodo para eliminar un producto
        /// </summary>
        /// <param name="id">Id del registro en la BD del producto a actualizar </param>
        /// <returns>Si se elimina o no el producto</returns>
        /// <response code="404">Si el producto no existe</response>
        [HttpDelete("EliminarProducto")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                if (id == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new ErrorModel { error = "Debe enviar un Id o un Número de Documento" });
                }
                else
                {
                    return Ok(ProductoHelper.EliminarProducto(id));
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
    }
}
