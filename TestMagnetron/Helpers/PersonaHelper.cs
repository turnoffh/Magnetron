using TestMagnetron.Models;
namespace TestMagnetron.Helpers
{
    /// <summary>
    /// Clase para procesar las peticiones
    /// </summary>
    public class PersonaHelper
    {
        public static IEnumerable<Persona> ObtenerPersonas()
        {
            using (var db = new TestMagnetronContext())
            {
                IEnumerable<Persona> personas = db.Personas.ToList();
                return personas;
            }
        }
    }
}
