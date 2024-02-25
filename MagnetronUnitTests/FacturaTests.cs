using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMagnetron.Controllers;
using TestMagnetron.Models;
using TestMagnetron;

namespace MagnetronUnitTests
{
    public class FacturaTests
    {
        private FactEncabezado facturaTest = new FactEncabezado();
        private FactDetalle detalleTest = new FactDetalle();
        private FacturaController controller = new FacturaController();
        [SetUp]
        public void Setup()
         {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Fact_Encabezado ON");
                        db.Database.ExecuteSqlRaw("INSERT INTO Fact_Encabezado (FEnc_ID, FEnc_Numero, FEnc_Fecha, FEnc_Per_ID) VALUES (999, 'TEST001', DATEADD(DAY, -1, GETDATE()), 1)");
                        db.Database.ExecuteSqlRaw("INSERT INTO Fact_Detalle (FDet_Linea, FDet_Cantidad, FDet_Prod_ID, FDet_FEnc_ID) VALUES ('TESTLinea 1', 1, 1, 999)");
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Fact_Encabezado OFF");
                        transaction.Commit();
                    }

                    facturaTest = new FactEncabezado
                    {
                        FencNumero = "TEST002",
                        FencFecha = DateTime.Now,
                        FencPerId = 1
                    };
                    db.FactEncabezados.Add(facturaTest);
                    db.SaveChanges();

                    detalleTest = new FactDetalle
                    {
                        FdetLinea = "TEST",
                        FdetCantidad = 1,
                        FdetProdId = 1,
                        FdetFencId = facturaTest.FencId
                    };
                    db.FactDetalles.Add(detalleTest); 
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para validar escenarios de agregar factura
        /// </summary>
        [Test]
        public void TestAgregarFactura()
        {
            try
            {
                FactEncabezadoInputVM factura = new FactEncabezadoInputVM()
                {
                    Numero = "TEST002",
                    Fecha = DateTime.Now,
                    PersonaId = 999,
                    Detalles = new List<FactDetalleInputVM>
                    {
                        new FactDetalleInputVM()
                        {
                            Linea = "TEST Linea",
                            Cantidad = 2,
                            ProductoId = 999
                        }
                    }
                };
                var response = controller.AgregarFactura(factura);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que la factura ya esta agregada");

                factura.Numero = "TEST Factura 001";
                response = controller.AgregarFactura(factura);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que la persona no existe");

                factura.PersonaId = 1;
                response = controller.AgregarFactura(factura);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que el producto del detalle no existe");

                factura.Detalles.ToList()[0].ProductoId = 1;
                response = controller.AgregarFactura(factura);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio agregar la factura");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las facturas almacenadas en BD
        /// </summary>
        [Test]
        public void TestObtenerFacturas()
        {
            try
            {
                var response = controller.ObtenerFacturas();
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio retornar Ok");
                List<FactEncabezado> pList = objRes.Value as List<FactEncabezado>;
                Assert.That(pList.Count(), Is.GreaterThanOrEqualTo(2), "Debe haber como minimo dos registros creados al iniciar el test");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las facturas por numero
        /// </summary>
        [Test]
        public void TestObtenerfacturasPorNumero()
        {
            try
            {
                var response = controller.ObtenerFacturaPorNumero("");
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que el id esta vacio");

                response = controller.ObtenerFacturaPorNumero("XYZ999");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(404), "Debio retornar error ya que el numero no existe");

                response = controller.ObtenerFacturaPorNumero("TEST001");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                FactEncabezado fact = objRes.Value as FactEncabezado;
                Assert.That(fact.FencNumero, Is.EqualTo("TEST001"), "Debe haber un registro encontrado con el parametro TEST001");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para Actualizar una factura validando los posibles escenarios
        /// </summary>
        [Test]
        public void TestActualizarFactura()
        {
            try
            {
                FactEncabezadoUpdateVM encabezado = new FactEncabezadoUpdateVM()
                {
                    Numero = "TEST Factura 002"
                };
                var response = controller.ActualizarFactura(0, encabezado);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id");

                response = controller.ActualizarFactura(9999999, encabezado);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(404), "Debio retornar error ya que el encabezado no existe");

                response = controller.ActualizarFactura(999, encabezado);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio Actualizar el encabezado de factura");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para eliminar la factura 
        /// </summary>
        [Test]
        public void TestEliminarProducto()
        {
            try
            {
                var response = controller.EliminarFactura(0);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id o documento");

                response = controller.EliminarFactura(99999);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(404), "Debio retornar error ya que la factura no existe");

                response = controller.EliminarFactura(999);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio eliminar la factura");
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
                    List<FactEncabezado> factTests = db.FactEncabezados.Where(fe => fe.FencNumero.StartsWith("TEST")).ToList();
                    foreach (FactEncabezado fact in factTests)
                    {
                        var detallesAEliminar = db.FactDetalles.Where(fd => fd.FdetFencId == fact.FencId).ToList();

                        // Eliminamos los detalles
                        db.FactDetalles.RemoveRange(detallesAEliminar);
                        db.SaveChanges();
                        db.FactEncabezados.Remove(fact);
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