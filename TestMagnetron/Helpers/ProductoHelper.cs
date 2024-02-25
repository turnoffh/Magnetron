using System.Runtime.ConstrainedExecution;
using TestMagnetron.Models;
namespace TestMagnetron.Helpers
{
    /// <summary>
    /// Clase para procesar las peticiones
    /// </summary>
    public class ProductoHelper
    {
        /// <summary>
        /// Metodo para agregar un producto a la BD
        /// </summary>
        /// <param name="prod">Producto para agregar</param>
        /// <returns>Producto agregado</returns>
        public static Producto AgregarProducto(ProductoInputVM prod)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    //validamos que la persona a agregar no exista en la base de datos
                    if (db.Productos.Any(p => p.ProdNombre == prod.ProdNombre && 
                                                p.ProdDescripcion == prod.ProdDescripcion &&
                                                p.ProdPrecio == prod.ProdPrecio &&
                                                p.ProdCosto == prod.ProdCosto &&
                                                p.ProdUm == prod.ProdUm))
                    {
                        throw new InvalidOperationException("El producto ya existe en la base de datos.");
                    }
                    Producto prodRet = new Producto
                    {
                        ProdNombre = prod.ProdNombre,
                        ProdDescripcion = prod.ProdDescripcion,
                        ProdPrecio = prod.ProdPrecio,
                        ProdCosto = prod.ProdCosto,
                        ProdUm = prod.ProdUm
                    };
                    db.Productos.Add(prodRet);
                    db.SaveChanges();
                    return prodRet; //retornamos el producto nuevo agregado
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para obtener una lista de productos
        /// </summary>
        /// <returns>Lista de productos en base de datos</returns>
        public static IEnumerable<Producto> ObtenerProductos()
        {
            try
            {
                using (var db = new TestMagnetronContext()) //Usamos el context de la base de datos
                {
                    IEnumerable<Producto> productos = db.Productos.ToList(); //obtenemos los productos almacenados y las transformamos a una lista
                    return productos;
                }
            }
            catch (Exception)
            {
                throw; // arrojamos la excepcion para ser agarrada en el catch del controller
            }
            
        }

        /// <summary>
        /// Metodo para obtener un producto por Id
        /// </summary>
        /// <param name="id">Id para buscar</param>
        /// <returns>Producto encontrado</returns>
        public static Producto ObtenerProductoPorId(int id)
        {
            try
            {
                using (var db = new TestMagnetronContext())//Usamos el context de la base de datos
                {
                    Producto producto = db.Productos.Where(p => p.ProdId == id).FirstOrDefault(); // buscamos si el id existe en base de datos
                    if (producto is null)
                    {
                        throw new InvalidOperationException("No se encontro ningun registro con el id '" + id + " '.");
                    }
                    return producto;
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        /// <summary>
        /// Metodo para obtener productos por nombre
        /// </summary>
        /// <param name="nombre">Nombre de la persona a buscar</param>
        /// <returns>Persona encontrada</returns>
        public static List<Producto> ObtenerProductoPorNombre(string nombre)
        {
            try
            {
                using (var db = new TestMagnetronContext())//Usamos el context de la base de datos
                {
                    List<Producto> productos = db.Productos.Where(p => p.ProdNombre.ToLower().Contains(nombre)).ToList(); // buscamos el nombre en base de datos 
                    if (productos is null || productos.Count == 0)
                    {
                        throw new InvalidOperationException("No se encontro ningun registro con el id '" + nombre + " '.");
                    }
                    return productos;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para actualizar un producto de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD del producto a actualizar</param>
        /// <param name="producto">Objeto con los valores nuevos a actualizar</param>
        /// <returns>Producto actualizado.</returns>
        public static Producto ActualizarProducto(int id, ProductoUpdateVM producto)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    Producto prodAct;
                    //validamos que el producto a actualizar exista en la base de datos
                    prodAct = db.Productos.Where(p => p.ProdId == id).FirstOrDefault();
                    
                    if (prodAct == null)
                    {
                        throw new InvalidOperationException("El producto a actualizar no existe en la base de datos.");
                    }

                    if (!string.IsNullOrEmpty(producto.ProdNombre) && prodAct.ProdNombre != prodAct.ProdNombre)
                    {
                        prodAct.ProdNombre = producto.ProdNombre;
                    }
                    if (!string.IsNullOrEmpty(producto.ProdDescripcion) && prodAct.ProdDescripcion != prodAct.ProdDescripcion)
                    {
                        prodAct.ProdDescripcion = producto.ProdDescripcion;
                    }
                    if (producto.ProdPrecio != 0 && prodAct.ProdPrecio != prodAct.ProdPrecio)
                    {
                        prodAct.ProdPrecio = producto.ProdPrecio;
                    }
                    if (producto.ProdCosto != 0 && prodAct.ProdCosto != prodAct.ProdCosto)
                    {
                        prodAct.ProdCosto = producto.ProdCosto;
                    }
                    if (!string.IsNullOrEmpty(producto.ProdUm) && prodAct.ProdUm != prodAct.ProdUm)
                    {
                        prodAct.ProdUm = producto.ProdUm;
                    }
                    db.SaveChanges();
                    return prodAct; //retornamos el producto actualizado
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para eliminar un producto
        /// </summary>
        /// <param name="id">Id del registro en la BD del producto a eliminar</param>
        /// <returns>Producto eliminado.</returns>
        public static string EliminarProducto(int id)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    Producto productoEliminado;
                    //validamos que el producto exista
                    productoEliminado = db.Productos.Where(p => p.ProdId == id).FirstOrDefault();
                    
                    if (productoEliminado == null)
                    {
                        throw new InvalidOperationException("El producto a eliminar no existe en la base de datos.");
                    }

                    db.Productos.Remove(productoEliminado);
                    db.SaveChanges();
                    return "Producto eliminado";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
