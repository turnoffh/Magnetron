using Microsoft.EntityFrameworkCore;
using TestMagnetron.Models;

namespace TestMagnetron.Helpers
{
    /// <summary>
    /// Clase para procesar las peticiones
    /// </summary>
    public class FacturaHelper
    {
        /// <summary>
        /// Metodo para agregar una factura a la BD
        /// </summary>
        /// <param name="facturaCompleta">Factura a agregar</param>
        /// <returns>La factura armada con los detalles</returns>
        public static FactEncabezado AgregarFactura(FactEncabezadoInputVM facturaCompleta)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    //validamos que la factura a agregar no exista en la base de datos
                    if (db.FactEncabezados.Any(f => f.FencNumero == facturaCompleta.Numero))
                    {
                        throw new InvalidOperationException("La factura ya existe en la base de datos, si desea actualizar el detalle por favor use el metodo actualizar.");
                    }
                    
                    //Validamos que el cliente exista
                    if (!db.Personas.Any(p => p.PerId == facturaCompleta.PersonaId))
                    {
                        throw new InvalidOperationException("El cliente de la factura no existe, registrar primero por favor.");
                    }

                    foreach (var f in facturaCompleta.Detalles)
                    {
                        if (!db.Productos.Any(p => p.ProdId == f.ProductoId))
                        {
                            throw new InvalidOperationException("El producto id '" + f.ProductoId + "' de la factura no existe, registrar primero por favor.");
                        }
                    }

                    FactEncabezado encab = new FactEncabezado
                    {
                        FencNumero = facturaCompleta.Numero,
                        FencFecha = facturaCompleta.Fecha,
                        FencPerId = facturaCompleta.PersonaId
                    };
                    db.FactEncabezados.Add(encab);
                    db.SaveChanges();
                    encab.FactDetalles = new List<FactDetalle>();
                    foreach (FactDetalleInputVM det in facturaCompleta.Detalles)
                    {
                        FactDetalle detalle = new FactDetalle()
                        {
                            FdetLinea = det.Linea,
                            FdetCantidad = det.Cantidad,
                            FdetProdId = det.ProductoId,
                            FdetFencId = encab.FencId
                        };
                        db.FactDetalles.Add(detalle); 
                        db.SaveChanges();
                        encab.FactDetalles.Add(detalle);
                    }
                    
                    return encab; //retornamos la factura nueva agregada
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para obtener una lista de facturas
        /// </summary>
        /// <returns>Lista de facturas en base de datos</returns>
        public static IEnumerable<FactEncabezado> ObtenerFacturas()
        {
            try
            {
                using (var db = new TestMagnetronContext()) //Usamos el context de la base de datos
                {
                    List<FactEncabezado> factEncabezados = db.FactEncabezados.ToList(); //obtenemos las facturas almacenadas y las transformamos a una lista
                    List<FactDetalle> factDetalles = db.FactDetalles.ToList(); //llamamos a los detalles para que nos cargue la propiedad virtual de factdetalles en el encabezado
                    List<Persona> personas = db.Personas.ToList();//llamamos a las personas para que nos cargue la propiedad virtual de FencPer en el encabezado
                    List<Producto> productos = db.Productos.ToList();//Llamamos a los productos para que nos cargue la propiedad virtual FdetProd en el detalle
                    return factEncabezados;
                }
            }
            catch (Exception)
            {
                throw; // arrojamos la excepcion para ser agarrada en el catch del controller
            }
            
        }

        /// <summary>
        /// Metodo para obtener una factura por número de factura
        /// </summary>
        /// <param name="fac">Número de factura a buscar</param>
        /// <returns>Factura encontrada</returns>
        public static FactEncabezado ObtenerFacturaPorNumero(string fac)
        {
            try
            {
                using (var db = new TestMagnetronContext())//Usamos el context de la base de datos
                {
                    FactEncabezado factura = db.FactEncabezados.Where(f => f.FencNumero.ToLower() == fac.ToLower()).FirstOrDefault(); // buscamos si el numero de factura existe en base de datos
                    if (factura is null)
                    {
                        throw new InvalidOperationException("No se encontro ningun registro con el id '" + fac + "'.");
                    }

                    List<FactDetalle> detalles = db.FactDetalles.Where(f => f.FdetFencId == factura.FencId).ToList();
                    Persona per = db.Personas.Where(p => p.PerId == factura.FencPerId).FirstOrDefault();
                    List<Producto> productos = db.Productos.Where(p => detalles.Select(d => d.FdetProdId).Contains(p.ProdId)).ToList();
                    return factura;
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        /// <summary>
        /// Metodo para actualizar el encabezado de una Factura de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD de la factura a actualizar</param>
        /// <param name="nuevoEncab">Nuevo objeto con los valores a actualizar de la factura</param>
        /// <returns>Encabezado de la factura actualizado.</returns>
        public static FactEncabezado ActualizarFactura(int id, FactEncabezadoUpdateVM nuevoEncab)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    //validamos que la factura a actualizar exista en la base de datos
                    FactEncabezado factActualizada = db.FactEncabezados.Where(f => f.FencId == id).FirstOrDefault();
                    if (factActualizada == null)
                    {
                        throw new InvalidOperationException("La factura a actualizar no existe en la base de datos.");
                    }

                    if (!string.IsNullOrEmpty(nuevoEncab.Numero) && factActualizada.FencNumero != nuevoEncab.Numero)
                    {
                        factActualizada.FencNumero = nuevoEncab.Numero;
                    }

                    DateTime fecha;
                    if (DateTime.TryParse(nuevoEncab.Fecha.ToString(), out fecha))
                    {
                        if (!nuevoEncab.Fecha.ToString().Contains("1/01/0001") && factActualizada.FencFecha != nuevoEncab.Fecha)
                        {
                            factActualizada.FencFecha = nuevoEncab.Fecha;
                        }
                    }                    

                    if (nuevoEncab.PersonaId != 0 && factActualizada.FencPerId != nuevoEncab.PersonaId)
                    {
                        Persona per = db.Personas.Where(p => p.PerId == nuevoEncab.PersonaId).FirstOrDefault();
                        if (per == null)
                        {
                            throw new InvalidOperationException("La persona con id '" + nuevoEncab.PersonaId + "' no existe, por favor crearla primero");
                        }
                        factActualizada.FencPerId = nuevoEncab.PersonaId;
                    }

                    db.SaveChanges();
                    List<FactDetalle> detalles = db.FactDetalles.Where(d => d.FdetFencId == factActualizada.FencId).ToList();
                    Persona per1 = db.Personas.Where(p => p.PerId == factActualizada.FencPerId).FirstOrDefault();
                    List<Producto> productos = db.Productos.Where(p => detalles.Select(d => d.FdetProdId).Contains(p.ProdId)).ToList();
                    return factActualizada; //retornamos la factura actualizada
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para actualizar el detalle de una Factura de la BD
        /// </summary>
        /// <param name="id">Id del registro del detalle de la factura a actualizar en la BD</param>
        /// <param name="nuevoDetalle">Nuevo objeto con los valores a actualizar del detalle de la factura</param>
        /// <returns>Detalle de la factura actualizado.</returns>
        public static FactDetalle ActualizarDetalleFactura(int id, FactDetalleUpdateVM nuevoDetalle)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    //validamos que el detalle de la factura a actualizar exista en la base de datos
                    FactDetalle detalleActualizado = db.FactDetalles.Where(f => f.FdetId == id).FirstOrDefault();
                    if (detalleActualizado == null)
                    {
                        throw new InvalidOperationException("El detalle de factura a actualizar no existe en la base de datos.");
                    }

                    if (!string.IsNullOrEmpty(nuevoDetalle.Linea) && detalleActualizado.FdetLinea != nuevoDetalle.Linea)
                    {
                        detalleActualizado.FdetLinea = nuevoDetalle.Linea;
                    }

                    if (nuevoDetalle.ProductoId != 0 && detalleActualizado.FdetProdId != nuevoDetalle.ProductoId)
                    {
                        Producto pro = db.Productos.Where(p => p.ProdId == nuevoDetalle.ProductoId).FirstOrDefault();
                        if (pro == null)
                        {
                            throw new InvalidOperationException("El producto con id '" + nuevoDetalle.ProductoId + "' no existe, por favor creelo primero");
                        }
                        detalleActualizado.FdetProdId = nuevoDetalle.ProductoId;
                    }

                    if (nuevoDetalle.Cantidad != 0 && detalleActualizado.FdetCantidad != nuevoDetalle.Cantidad)
                    {
                        detalleActualizado.FdetCantidad = nuevoDetalle.Cantidad;
                    }

                    db.SaveChanges();
                    Producto pro1 = db.Productos.Where(p => p.ProdId == nuevoDetalle.ProductoId).FirstOrDefault(); //Traemos el producto para cargarlo en la propiedad virtual del detalle de la factura, se vuelve a llamar por si no se cambio el productoid.
                    return detalleActualizado; //retornamos la factura actualizada
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para eliminar una factura de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD de la factura a eliminar (opcional)</param>
        /// <param name="numero">Numero de la factura a eliminar (opcional)</param>
        /// <returns>Factura Eliminada.</returns>
        public static string EliminarFactura(int id, string? numero)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    FactEncabezado factEliminada;
                    //validamos que la factura a eliminar exista en la base de datos
                    if (id > 0)
                    {
                        factEliminada = db.FactEncabezados.Where(f => f.FencId == id).FirstOrDefault();
                    }
                    else
                    {
                        factEliminada = db.FactEncabezados.Where(f => f.FencNumero.ToLower() == numero.ToLower()).FirstOrDefault();
                    }
                    if (factEliminada == null)
                    {
                        throw new InvalidOperationException("La factura a eliminar no existe en la base de datos.");
                    }

                    // Seleccionamos los detalles pertenecientes a la factura a eliminar para evitar el error de llave foranea
                    var detallesAEliminar = db.FactDetalles.Where(fd => fd.FdetFencId == factEliminada.FencId).ToList();

                    // Eliminamos los detalles
                    db.FactDetalles.RemoveRange(detallesAEliminar);
                    db.SaveChanges();
                    db.FactEncabezados.Remove(factEliminada);
                    db.SaveChanges();
                    return "Factura eliminada";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para eliminar un detalle de una factura de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD del detalle de la factura a eliminar (opcional)</param>
        /// <returns>Detalle Eliminado.</returns>
        public static string EliminarDetalleFactura(int id)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    FactDetalle detalleEliminado;
                    //validamos que el detalle de la factura a eliminar exista en la base de datos
                    detalleEliminado = db.FactDetalles.Where(f => f.FdetId == id).FirstOrDefault();
                    
                    if (detalleEliminado == null)
                    {
                        throw new InvalidOperationException("El detalle de la factura a eliminar no existe en la base de datos.");
                    }

                    // Eliminamos los detalles
                    db.FactDetalles.Remove(detalleEliminado);
                    db.SaveChanges();
                    return "Detalle eliminado";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
