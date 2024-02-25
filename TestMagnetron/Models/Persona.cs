using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestMagnetron.Models
{
    public partial class Persona
    {
        public Persona()
        {
            FactEncabezados = new HashSet<FactEncabezado>();
        }

        public int PerId { get; set; }
        public string? PerNombre { get; set; }
        public string? PerApellido { get; set; }
        public int PerTipoDocumento { get; set; }
        public string PerDocumento { get; set; } = null!;

        [JsonIgnore]
        public virtual TiposDocumento PerTipoDocumentoNavigation { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<FactEncabezado> FactEncabezados { get; set; }
    }

    public class PersonaInputVM
    {
        public string? PerNombre { get; set; }
        public string? PerApellido { get; set; }
        public int PerTipoDocumento { get; set; }
        public string PerDocumento { get; set; } = null!;
    }

    public class PersonaUpdateVM
    {
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public string? PerNombre { get; set; }
        [BindNever]
        public string? PerApellido { get; set; }
        [BindNever]
        public int PerTipoDocumento { get; set; }
    }
}
