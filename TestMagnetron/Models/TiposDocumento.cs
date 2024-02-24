using System;
using System.Collections.Generic;

namespace TestMagnetron.Models
{
    public partial class TiposDocumento
    {
        public TiposDocumento()
        {
            Personas = new HashSet<Persona>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
