using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestMagnetron.Models
{
    public partial class Producto
    {
        public Producto()
        {
            FactDetalles = new HashSet<FactDetalle>();
        }

        public int ProdId { get; set; }
        public string ProdNombre { get; set; } = null!;
        public string ProdDescripcion { get; set; } = null!;
        public decimal ProdPrecio { get; set; }
        public decimal ProdCosto { get; set; }
        public string? ProdUm { get; set; }

        [JsonIgnore]
        public virtual ICollection<FactDetalle> FactDetalles { get; set; }
    }

    public class ProductoInputVM
    {
        public string ProdNombre { get; set; } = null!;
        public string ProdDescripcion { get; set; } = null!;
        public decimal ProdPrecio { get; set; }
        public decimal ProdCosto { get; set; }
        public string? ProdUm { get; set; }
    }

    public class ProductoUpdateVM
    {
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public string ProdNombre { get; set; } = null!;
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public string ProdDescripcion { get; set; } = null!;
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public decimal ProdPrecio { get; set; }
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public decimal ProdCosto { get; set; }
        [BindNever] //Se usa para indicar al MVC que no valide que venga este campo
        public string? ProdUm { get; set; }
    }
}
