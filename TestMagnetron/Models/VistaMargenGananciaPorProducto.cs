using System;
using System.Collections.Generic;

namespace TestMagnetron.Models
{
    public partial class VistaMargenGananciaPorProducto
    {
        public int ProdId { get; set; }
        public string ProdNombre { get; set; } = null!;
        public decimal? MargenGanancia { get; set; }
    }
}
