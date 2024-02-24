using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.ConstrainedExecution;
using TestMagnetron;
using TestMagnetron.Controllers;
using TestMagnetron.Models;

namespace MagnetronUnitTests
{
    public class PersonaTests
    {
        private Persona personaTest = new Persona();
        private PersonaController controller = new PersonaController();
        [SetUp]
        public void Setup()
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    personaTest = new Persona
                    {
                        PerNombre = "TEST User",
                        PerApellido = "Test Apellido",
                        PerTipoDocumento = 1,
                        PerDocumento = "ABC123456789"
                    };
                    db.Personas.Add(personaTest);
                    db.SaveChanges();

                    personaTest = new Persona
                    {
                        PerNombre = "TEST User 2",
                        PerApellido = "Test Apellido 2",
                        PerTipoDocumento = 2,
                        PerDocumento = "ABC1223456789"
                    };
                    db.Personas.Add(personaTest);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para validar escenarios de agregar persona
        /// </summary>
        [Test]
        public void TestAgregarPersona()
        {
            try
            {
                PersonaInputVM persona = new PersonaInputVM()
                {
                    PerNombre = "TEST Samuel",
                    PerApellido = "Hernandez",
                    PerTipoDocumento = 20,
                    PerDocumento = "ABC123456789"
                };
                var response = controller.AddUser(persona);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(500), "Debio retornar error ya que el tipo de documento no existe");

                persona.PerTipoDocumento = 1;
                response = controller.AddUser(persona);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que la persona ya esta agregada");

                persona.PerDocumento = "XYZ789";
                response = controller.AddUser(persona);
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio agregar a la persona");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las personas almacenadas en BD
        /// </summary>
        [Test]
        public void TestObtenerPersonas()
        {
            try
            {
                var response = controller.GetPersonas();
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio retornar error ya que el tipo de documento no existe");
                List<Persona> pList = objRes.Value as List<Persona>;
                Assert.That(pList.Count(), Is.GreaterThanOrEqualTo(2), "Debe haber como minimo dos registros creados en el setup");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las personas por documento
        /// </summary>
        [Test]
        public void TestObtenerPersonasPorDoc()
        {
            try
            {
                var response = controller.GetPersonasPorDocumento("");
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que el Documento esta vacio");
                response = controller.GetPersonasPorDocumento("ABC");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                List<Persona> pList = objRes.Value as List<Persona>;
                Assert.That(pList.Count, Is.GreaterThanOrEqualTo(2), "Debe haber como minimo dos registros encontrados con el parametro ABC");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para traer las personas por nombre
        /// </summary>
        [Test]
        public void TestObtenerPersonasPorNombre()
        {
            try
            {
                var response = controller.GetPersonasPorNombreoApellido("");
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que el Documento esta vacio");
                response = controller.GetPersonasPorNombreoApellido("tEsT UsEr 2");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                List<Persona> pList = objRes.Value as List<Persona>;
                Assert.That(pList.Count, Is.GreaterThanOrEqualTo(1), "Debe haber como minimo un registro encontrado");

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para Actualizar la persona validando los posibles escenarios
        /// </summary>
        [Test]
        public void TestActualizarPersona()
        {
            try
            {
                PersonaUpdateVM per = new PersonaUpdateVM()
                {
                    PerNombre = "David",
                    PerTipoDocumento = 999
                };
                var response = controller.UpdatePersona(per);
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id o documento");
                response = controller.UpdatePersona(per, 0, "XYZ");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que la persona no existe");
                response = controller.UpdatePersona(per, 0, "ABC1223456789");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(500), "Debio retornar error ya que el tipo de documento no es valido");
                per.PerTipoDocumento = 2;
                response = controller.UpdatePersona(per, 0, "ABC1223456789");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(200), "Debio Actualizar la persona");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Test para eliminar la persona por documento validando los posibles escenarios
        /// </summary>
        [Test]
        public void TestEliminarPersona()
        {
            try
            {
                var response = controller.DeletePersona(0,"");
                Assert.IsNotNull(response);
                ObjectResult objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(400), "Debio retornar error ya que es necesario el id o documento");
                response = controller.DeletePersona(0, "XYZ");
                Assert.IsNotNull(response);
                objRes = response as ObjectResult;
                Assert.That(objRes.StatusCode, Is.EqualTo(409), "Debio retornar error ya que la persona no existe");
                response = controller.DeletePersona( 0, "ABC1223456789");
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
                    List<Persona> perTests = db.Personas.Where(persona => persona.PerNombre.StartsWith("TEST") || persona.PerApellido.StartsWith("TEST")).ToList();
                    foreach (Persona per in perTests)
                    {
                        db.Personas.Remove(per);
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