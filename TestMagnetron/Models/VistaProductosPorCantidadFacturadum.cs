using System;
using System.Collections.Generic;

namespace TestMagnetron.Models
{
    public partial class VistaProductosPorCantidadFacturadum
    {
        public int ProdId { get; set; }
        public string ProdNombre { get; set; } = null!;
        public int? CantidadFacturada { get; set; }
    }
}
