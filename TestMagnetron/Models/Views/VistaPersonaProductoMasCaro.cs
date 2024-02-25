using System;
using System.Collections.Generic;

namespace TestMagnetron.Models.Views
{
    public partial class VistaPersonaProductoMasCaro
    {
        public int PerId { get; set; }
        public string? Descripcion { get; set; }
        public string PerDocumento { get; set; } = null!;
        public string? Cliente { get; set; }
        public string Producto { get; set; } = null!;
        public decimal PrecioProducto { get; set; }
    }
}
