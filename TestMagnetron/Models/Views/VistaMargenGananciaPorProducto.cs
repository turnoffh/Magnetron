using System;
using System.Collections.Generic;

namespace TestMagnetron.Models.Views
{
    public partial class VistaMargenGananciaPorProducto
    {
        public int ProdId { get; set; }
        public string ProdNombre { get; set; } = null!;
        public decimal? MargenGanancia { get; set; }
    }
}
