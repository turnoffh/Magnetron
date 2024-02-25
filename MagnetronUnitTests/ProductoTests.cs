using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMagnetron.Controllers;
using TestMagnetron.Models;

namespace MagnetronUnitTests
{
    public class ProductoTests
    {
        private Producto productoTest = new Producto();
        private ProductoController controller = new ProductoController();
        [SetUp]
        public void Setup()
         {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Producto ON");
                        db.Database.ExecuteSqlRaw("INSERT INTO dbo.Producto (Prod_Id, Prod_Nombre, Prod_Descripcion, Prod_Precio, Prod_Costo, Prod_Um) " +
                                                  "VALUES (999, 'TEST Producto', 'Test Descripcion', 10, 5, 'Kg')");
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Producto OFF");
                        transaction.Commit();
                    }
                    
                    productoTest = new Producto
                    {
                        ProdNombre = "TEST Producto 2",
                        ProdDescripcion = "Test Descripcion 2",
                        ProdPrecio = 10,
                        ProdCosto = 5,
                        ProdUm = "Kg"
                    };
                    db.Productos.Add(productoTest);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para validar escenarios de agregar producto
        /// </summary>
        [Test]
        public void TestAgregarProducto()
        {
            try
            {
                ProductoInputVM producto = new ProductoInputVM()
                {
                    ProdNombre = "TEST Producto 2",
                    ProdDescripcion = "Test Descripcion 2",
                    ProdPrecio = 10,
                    ProdCosto = 5,
                    ProdUm = "Kg"
                };
                var response = controller.AgregarProducto(producto);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que el producto ya esta agregado");

                producto.ProdNombre = "TEST Producto 3";
                response = controller.AgregarProducto(producto);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio agregar el producto");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer los productos almacenados en BD
        /// </summary>
        [Test]
        public void TestObtenerProductos()
        {
            try
            {
                var response = controller.ObtenerProductos();
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio retornar Ok");
                List<Producto> pList = objRes.Value as List<Producto>;
                Assert.That(pList.Count(), Is.GreaterThanOrEqualTo(2), "Debe haber como minimo dos registros creados al iniciar el test");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer los productos por id
        /// </summary>
        [Test]
        public void TestObtenerProductoPorId()
        {
            try
            {
                var response = controller.ObtenerProductoPorId(0);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que el id esta vacio");
                response = controller.ObtenerProductoPorId(999);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Producto product = objRes.Value as Producto;
                Assert.That(product.ProdId, Is.EqualTo(999), "Debe haber un registro encontrado con el parametro 999");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las productos por nombre
        /// </summary>
        [Test]
        public void TestObtenerProductosPorNombre()
        {
            try
            {
                var response = controller.ObtenerProductosPorNombre("");
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que el nombre a buscar esta vacio");
                response = controller.ObtenerProductosPorNombre("tEsT");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                List<Producto> pList = objRes.Value as List<Producto>;
                Assert.That(pList.Count, Is.GreaterThanOrEqualTo(2), "Debe haber como minimo dos registros encontrados");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para Actualizar un producto validando los posibles escenarios
        /// </summary>
        [Test]
        public void TestActualizarProducto()
        {
            try
            {
                ProductoUpdateVM producto = new ProductoUpdateVM()
                {
                    ProdNombre = "TEST David",
                    ProdDescripcion = "TEST David Description"
                };
                var response = controller.ActualizarProducto(0, producto);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id");
                response = controller.ActualizarProducto(9999999, producto);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(404), "Debio retornar error ya que el producto no existe");
                response = controller.ActualizarProducto(999, producto);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio Actualizar el producto");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para eliminar el producto por id validando los posibles escenarios
        /// </summary>
        [Test]
        public void TestEliminarProducto()
        {
            try
            {
                var response = controller.EliminarProducto(0);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id o documento");
                response = controller.EliminarProducto(99999);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(404), "Debio retornar error ya que el producto no existe");
                response = controller.EliminarProducto( 999);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio eliminar la persona");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    List<Producto> prodTests = db.Productos.Where(producto => producto.ProdNombre.StartsWith("TEST")).ToList();
                    foreach (Producto prod in prodTests)
                    {
                        db.Productos.Remove(prod);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}