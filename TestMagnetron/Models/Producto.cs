using System;
using System.Collections.Generic;

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

        public virtual ICollection<FactDetalle> FactDetalles { get; set; }
    }
}
