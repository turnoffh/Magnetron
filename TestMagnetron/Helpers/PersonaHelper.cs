using TestMagnetron.Models;
namespace TestMagnetron.Helpers
{
    /// <summary>
    /// Clase para procesar las peticiones
    /// </summary>
    public class PersonaHelper
    {
        /// <summary>
        /// Metodo para agregar una persona a la BD
        /// </summary>
        /// <param name="per">Persona para agregar</param>
        /// <returns>Persona agregada</returns>
        public static Persona AgregarPersona(PersonaInputVM per)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    //validamos que la persona a agregar no exista en la base de datos
                    if (db.Personas.Any(p => p.PerDocumento == per.PerDocumento && p.PerTipoDocumento == per.PerTipoDocumento))
                    {
                        throw new InvalidOperationException("La persona ya existe en la base de datos.");
                    }
                    Persona perRet = new Persona
                    {
                        PerNombre = per.PerNombre,
                        PerApellido = per.PerApellido,
                        PerTipoDocumento = per.PerTipoDocumento,
                        PerDocumento = per.PerDocumento
                    };
                    db.Personas.Add(perRet);
                    db.SaveChanges();
                    return perRet; //retornamos la persona nueva agregada
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para obtener una lista de personas
        /// </summary>
        /// <returns>Lista de personas en base de datos</returns>
        public static IEnumerable<Persona> ObtenerPersonas()
        {
            try
            {
                using (var db = new TestMagnetronContext()) //Usamos el context de la base de datos
                {
                    IEnumerable<Persona> personas = db.Personas.ToList(); //obtenemos las personas almacenadas y las transformamos a una lista
                    return personas;
                }
            }
            catch (Exception)
            {
                throw; // arrojamos la excepcion para ser agarrada en el catch del controller
            }
            
        }

        /// <summary>
        /// Metodo para obtener una persona por número de documento
        /// </summary>
        /// <param name="doc">Número de documento a buscar</param>
        /// <returns>Lista de Personas encontrada</returns>
        public static List<Persona> ObtenerPersonaPorDocumento(string doc)
        {
            try
            {
                using (var db = new TestMagnetronContext())//Usamos el context de la base de datos
                {
                    List<Persona> persona = db.Personas.Where(p => p.PerDocumento.ToLower().Contains(doc.ToLower())).ToList(); // buscamos si el documento existe en base de datos
                    if (persona is null || persona.Count == 0)
                    {
                        throw new InvalidOperationException("No se encontro ningun registro con el id '" + doc + " '.");
                    }
                    return persona;
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        /// <summary>
        /// Metodo para obtener una persona por nombre
        /// </summary>
        /// <param name="nombre">Nombre de la persona a buscar</param>
        /// <returns>Persona encontrada</returns>
        public static List<Persona> ObtenerPersonaPorNombreOApellido(string nombre)
        {
            try
            {
                using (var db = new TestMagnetronContext())//Usamos el context de la base de datos
                {
                    List<Persona> persona = db.Personas.Where(p => p.PerNombre.ToLower().Contains(nombre) || p.PerApellido.ToLower().Contains(nombre)).ToList(); // buscamos el nombre en base de datos 
                    if (persona is null || persona.Count == 0)
                    {
                        throw new InvalidOperationException("No se encontro ningun registro con el parametro '" + nombre + " '.");
                    }
                    return persona;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para actualizar una persona de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD de la persona a actualizar (opcional)</param>
        /// <param name="documento">Documento de la persona a actualizar (opcional)</param>
        /// <param name="persona">Objeto con los valores nuevos a actualizar</param>
        /// <returns>Persona actualizada.</returns>
        public static Persona ActualizarPersona(PersonaUpdateVM persona, int id, string? documento)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    Persona perActualizada;
                    //validamos que la persona a actualizar exista en la base de datos
                    if (id > 0 )
                    {
                        perActualizada = db.Personas.Where(p => p.PerId == id).FirstOrDefault();
                    }
                    else
                    {
                        perActualizada = db.Personas.Where(p => p.PerDocumento.ToLower() == documento.ToLower()).FirstOrDefault();
                    }
                    if (perActualizada == null)
                    {
                        throw new InvalidOperationException("La persona a actualizar no existe en la base de datos.");
                    }

                    if (!string.IsNullOrEmpty(persona.PerNombre) && perActualizada.PerNombre != persona.PerNombre)
                    {
                        perActualizada.PerNombre = persona.PerNombre;
                    }

                    if (!string.IsNullOrEmpty(persona.PerApellido) && perActualizada.PerApellido != persona.PerApellido)
                    {
                        perActualizada.PerApellido = persona.PerApellido;
                    }

                    if (persona.PerTipoDocumento != 0 && perActualizada.PerTipoDocumento != persona.PerTipoDocumento)
                    {
                        perActualizada.PerTipoDocumento = persona.PerTipoDocumento;
                    }
                    db.SaveChanges();
                    return perActualizada; //retornamos la persona actualizada
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Metodo para eliminar una persona de la BD
        /// </summary>
        /// <param name="id">Id del registro en la BD de la persona a eliminar (opcional)</param>
        /// <param name="documento">Documento de la persona a eliminar (opcional)</param>
        /// <returns>Persona Eliminada.</returns>
        public static string EliminarPersona(int id, string? documento)
        {
            try
            {
                using (var db = new TestMagnetronContext())
                {
                    Persona perEliminada;
                    //validamos que la persona a eliminar exista en la base de datos
                    if (id > 0)
                    {
                        perEliminada = db.Personas.Where(p => p.PerId == id).FirstOrDefault();
                    }
                    else
                    {
                        perEliminada = db.Personas.Where(p => p.PerDocumento.ToLower() == documento.ToLower()).FirstOrDefault();
                    }
                    if (perEliminada == null)
                    {
                        throw new InvalidOperationException("La persona a eliminar no existe en la base de datos.");
                    }

                    db.Personas.Remove(perEliminada);
                    db.SaveChanges();
                    return "Persona eliminada";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
